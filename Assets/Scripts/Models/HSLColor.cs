using UnityEngine;

namespace Assets.Scripts.Models
{
    public class HslColor
    {
        private readonly int _hue;
        private readonly float _saturation;
        private readonly float _lightness;

        public HslColor(int hue, float saturation, float lightness)
        {
            _hue = hue;
            _saturation = saturation;
            _lightness = lightness;
        }

        public Color ToRgbColor()
        {
            var c = _saturation * (_lightness < 0.5 ? _lightness: 1 - _lightness);
            var s = 2 * c / (_lightness + c);
            var v = _lightness + _saturation;

            return Color.HSVToRGB(_hue, s, v);
        }
    }
}
