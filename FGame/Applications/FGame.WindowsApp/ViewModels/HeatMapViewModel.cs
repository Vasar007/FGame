using System;
using System.Windows.Media;
using Prism.Mvvm;
using FGame.Models;
using FGame.WindowsApp.Domain;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class HeatMapViewModel : BindableBase
    {
        private readonly double _min;

        private readonly double _max;

        public WorldPos.WorldPos Pos { get; }
        
        public double Value { get; }
        
        public string Text => $"{Pos.X}, {Pos.Y} = {Value:F2}";

        // Subtract 1 since data's indexes start at 1 instead of 0.
        public int PosX => (Pos.X - 1) * 10;
        
        public int PosY => (Pos.Y - 1) * 10;

        public Brush Fill => ProvideBrush();


        public HeatMapViewModel(WorldPos.WorldPos pos, double value, double min, double max)
        {
            Pos = pos.ThrowIfNull(nameof(pos));
            Value = value;
            _min = min;
            _max = max;
        }

        private Brush ProvideBrush()
        {
            double val = Value;
            double min = _min;
            double max = _max;

            if (min < 0)
            {
                max -= min;
                val -= min;
            }

            var rgb = (byte) Math.Round((val / max) * 255);
            var color = Color.FromRgb(rgb, rgb, rgb);
            return new SolidColorBrush(color);
        }
    }
}
