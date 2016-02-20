using System.IO;
using Assets.Scripts.Extensions;
using Assets.Scripts.ServerConnection;
using Assets.Scripts.Services;
using Autofac;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bootstrapper : MonoBehaviour 
    {
        public void Awake() 
        {
            var settings = File.ReadAllText(Path.Combine(Application.dataPath, "settings.txt"));
            var gameSettings = settings.FromJson<GameSettings>();

            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterInstance(gameSettings).As<GameSettings>();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}