using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace MauiApp1; 

public partial class MainPage : ContentPage
{
    // Словарь шаблонов цифр (5x7)
    private readonly Dictionary<char, int[,]> _digitPatterns = new Dictionary<char, int[,]>
    {
        {'0', new int[,] { {1,1,1,1,1}, {1,0,0,0,1}, {1,0,0,1,1}, {1,0,1,0,1}, {1,1,0,0,1}, {1,0,0,0,1}, {1,1,1,1,1} }},
        {'1', new int[,] { {0,0,1,0,0}, {0,1,1,0,0}, {0,0,1,0,0}, {0,0,1,0,0}, {0,0,1,0,0}, {0,0,1,0,0}, {0,1,1,1,0} }},
        {'2', new int[,] { {0,1,1,1,0}, {1,0,0,0,1}, {0,0,0,0,1}, {0,0,0,1,0}, {0,0,1,0,0}, {0,1,0,0,0}, {1,1,1,1,1} }},
        {'3', new int[,] { {1,1,1,1,0}, {0,0,0,0,1}, {0,0,0,0,1}, {0,1,1,1,0}, {0,0,0,0,1}, {0,0,0,0,1}, {1,1,1,1,0} }},
        {'4', new int[,] { {0,0,0,1,0}, {0,0,1,1,0}, {0,1,0,1,0}, {1,0,0,1,0}, {1,1,1,1,1}, {0,0,0,1,0}, {0,0,0,1,0} }},
        {'5', new int[,] { {1,1,1,1,1}, {1,0,0,0,0}, {1,1,1,1,0}, {0,0,0,0,1}, {0,0,0,0,1}, {1,0,0,0,1}, {0,1,1,1,0} }},
        {'6', new int[,] { {0,1,1,1,1}, {1,0,0,0,0}, {1,0,0,0,0}, {1,1,1,1,0}, {1,0,0,0,1}, {1,0,0,0,1}, {0,1,1,1,0} }},
        {'7', new int[,] { {1,1,1,1,1}, {0,0,0,0,1}, {0,0,0,1,0}, {0,0,1,0,0}, {0,1,0,0,0}, {0,1,0,0,0}, {0,1,0,0,0} }},
        {'8', new int[,] { {0,1,1,1,0}, {1,0,0,0,1}, {1,0,0,0,1}, {0,1,1,1,0}, {1,0,0,0,1}, {1,0,0,0,1}, {0,1,1,1,0} }},
        {'9', new int[,] { {0,1,1,1,0}, {1,0,0,0,1}, {1,0,0,0,1}, {0,1,1,1,1}, {0,0,0,0,1}, {0,0,0,0,1}, {1,1,1,1,0} }},
        {':', new int[,] { {0}, {1}, {0}, {0}, {1}, {0}, {0} }}
    };

    private IDispatcherTimer _timer;

    public MainPage()
    {
        InitializeComponent();

        ClockCanvas.Drawable = new ClockDrawable(_digitPatterns);

        // Таймер обновления каждую секунду
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (s, e) => { ClockCanvas.Invalidate(); };
        _timer.Start();
    }
}

// Класс для отрисовки
public class ClockDrawable : IDrawable
{
    private readonly Dictionary<char, int[,]> _patterns;

    public ClockDrawable(Dictionary<char, int[,]> patterns)
    {
        _patterns = patterns;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Colors.Black;
        canvas.FillRectangle(dirtyRect);

        canvas.FillColor = Colors.Cyan; // Цвет точек

        string timeString = DateTime.Now.ToString("HH:mm:ss");

        // Настройки размера 
        float dotSize = dirtyRect.Width / 90;
        float spacing = dotSize * 0.4f;
        float digitWidth = (5 * dotSize) + (4 * spacing);

        float totalWidth = (timeString.Length * (digitWidth + spacing * 2));
        float startX = (dirtyRect.Width - totalWidth) / 2;
        float startY = (dirtyRect.Height - (7 * (dotSize + spacing))) / 2;

        float currentX = startX;

        foreach (char c in timeString)
        {
            if (_patterns.ContainsKey(c))
            {
                var pattern = _patterns[c];

                for (int row = 0; row < pattern.GetLength(0); row++)
                {
                    for (int col = 0; col < pattern.GetLength(1); col++)
                    {
                        if (pattern[row, col] == 1)
                        {
                            float x = currentX + (col * (dotSize + spacing));
                            float y = startY + (row * (dotSize + spacing));
                            canvas.FillRectangle(x, y, dotSize, dotSize);
                        }
                    }
                }
                currentX += digitWidth + (spacing * 4);
            }
        }
    }
}