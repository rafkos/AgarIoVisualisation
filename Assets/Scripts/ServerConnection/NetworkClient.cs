using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using AgarIo.Contract;
using AgarIo.Contract.AdminCommands;
using AgarIo.Contract.PlayerCommands;
using Assets.Scripts.Extensions;
using Assets.Scripts.Services;
using UnityEngine;
using Action = System.Action;

namespace Assets.Scripts.ServerConnection
{
    public class NetworkClient : MonoBehaviour
    {
        private static readonly object SynchronizationLock = new object();
        private static readonly Queue<Action> ExecuteOnMainThread = new Queue<Action>();
        private GameEventPublisher _gameEventPublisher;
        private GameSettings _gameSettings;
        private Thread _thread;
        private bool _finish;

        public void Awake()
        {
            var eventAggregator = DependencyResolver.Current.GetService<IEventAggregator>();
            _gameEventPublisher = new GameEventPublisher(eventAggregator, ExecuteOnMainThread);
            _gameSettings = DependencyResolver.Current.GetService<GameSettings>();
        }

        public void Start()
        {
            _finish = false;
            _thread = new Thread(ThreadEntryPoint);
            _thread.Start(_gameSettings);
        }
        
        public void Update()
        {
            lock (SynchronizationLock)
            {
                while (ExecuteOnMainThread.Count > 0)
                {
                    ExecuteOnMainThread.Dequeue().Invoke();
                }
            }
        }

        public void OnDestroy()
        {
            if (_thread == null || !_thread.IsAlive)
            {
                return;
            }

            _finish = true;
            if (!_thread.Join(TimeSpan.FromSeconds(5)))
            {
                _thread.Abort();
            }
        }


        private void ThreadEntryPoint(object arg)
        {
            var threadParams = (GameSettings)arg;

            var tcpClient = new TcpClient { NoDelay = true };
            tcpClient.Connect(threadParams.Host, threadParams.Port);

            using (var writer = new StreamWriter(tcpClient.GetStream()))
            {
                writer.AutoFlush = true;
                using (var reader = new StreamReader(tcpClient.GetStream()))
                {
                    try
                    {
                        SendLoginData(reader, writer, threadParams.UserName, threadParams.Password, threadParams.IsAdmin, threadParams.IsVisualization);
                        HandleConnection(reader, writer);
                    }
                    catch (Exception e)
                    {
                        ExecuteOnMainThread.Enqueue(() => { throw e; });
                    }
                }
            }
        }

        private void HandleConnection(TextReader reader, TextWriter writer)
        {
            var startPushDto = new StartPushingStateAdminCommandDto();
            var startPushJson = startPushDto.ToJson();

            writer.WriteLine(startPushJson);
            reader.ReadLine();

            while (!_finish)
            {
                var pushDataJson = reader.ReadLine();
                var pushData = pushDataJson.FromJson<StatePushDto>();

                lock (SynchronizationLock)
                {
                    _gameEventPublisher.Handle(pushData);
                }
            }
        }

        private void SendLoginData(TextReader reader, TextWriter writer, string userName, string password, bool isAdmin, bool IsVisualization)
        {
            var loginDto = new LoginDto { Login = userName, Password = password, IsAdmin = isAdmin, IsVisualization = IsVisualization};
            var loginJson = loginDto.ToJson();

            writer.WriteLine(loginJson);
            var response = reader.ReadLine().FromJson<CommandResponseDto>();

            if (response.ErrorCode == 0)
            {
                return;
            }
            lock (SynchronizationLock)
            {
                ExecuteOnMainThread.Enqueue(() => { throw new Exception("Error Code: " + response.ErrorCode + " Message: " + response.Message); });
            }

            _thread.Abort();
        }
    }
}