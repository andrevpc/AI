using System;
using System.Drawing;

using Pamella;

public class Car
{
    public float Points { get; set; } = 0;
    public Color Color { get; set; } = Color.Orange;

    public float X => x;
    public float Y => y;

    float x, y;
    float xb, yb;

    float vx = 0, vy = 0;
    float vxb = 0, vyb = 0;

    float ax = 0f, ay = 0f;
    float axb = 0f, ayb = 0f;

    float dir = 0f;

    float power = 0f;
    float brake = 0f;
    float springLen = 120f;
    float springK = 100;

    public Car(float x0, float y0)
    {
        x = x0;
        xb = x0 - 120;
        y = y0;
        yb = y0;
    }

    public void Draw(IGraphics g, float camx, float camy, float zoom, float fit)
    {
        var ux = x - xb;
        var uy = y - yb;
        var dist = MathF.Sqrt(ux * ux + uy * uy);
        ux /= dist;
        uy /= dist;

        var brush = new SolidBrush(this.Color);

        float _x = x;
        float _y = y;
        float delta = dist / 16;
        for (int i = 0; i < 8; i++)
        {
            g.FillPolygon(new PointF[] {
                    new PointF(
                        zoom * (_x + 40 * uy) - camx,
                        zoom * (_y + 40 * ux) - camy
                    ),
                    new PointF(
                        zoom * (_x - 40 * uy - (delta - 5) * ux) - camx,
                        zoom * (_y - (delta - 5) * uy - 40 * ux) - camy
                    ),
                    new PointF(
                        zoom * (_x - 40 * uy - (delta + 5) * ux) - camx,
                        zoom * (_y - (delta + 5) * uy - 40 * ux) - camy
                    )
                },
                Brushes.Gray
            );
            _x -= ux * delta;
            _y -= uy * delta;

            g.FillPolygon(new PointF[] {
                    new PointF(
                        zoom * (_x - 40 * uy) - camx,
                        zoom * (_y - 40 * ux) - camy
                    ),
                    new PointF(
                        zoom * (_x + 40 * uy - (delta - 5) * ux) - camx,
                        zoom * (_y - (delta - 5) * uy + 40 * ux) - camy
                    ),
                    new PointF(
                        zoom * (_x + 40 * uy - (delta + 5) * ux) - camx,
                        zoom * (_y - (delta + 5) * uy + 40 * ux) - camy
                    )
                },
                Brushes.DarkGray
            );
            _x -= ux * delta;
            _y -= uy * delta;
        }

        g.FillPolygon(new PointF[] {
            new PointF(
                zoom * (x + 50 * ux - 50 * uy) - camx,
                zoom * (y + 50 * ux + 50 * uy) - camy
            ),
            new PointF(
                zoom * (x + 50 * ux + 50 * uy) - camx,
                zoom * (y - 50 * ux + 50 * uy) - camy
            ),
            new PointF(
                zoom * (x - 50 * ux + 50 * uy) - camx,
                zoom * (y - 50 * ux - 50 * uy) - camy
            ),
            new PointF(
                zoom * (x - 50 * ux - 50 * uy) - camx,
                zoom * (y + 50 * ux - 50 * uy) - camy
            ),
        }, brush);

        g.FillPolygon(new PointF[] {
            new PointF(
                zoom * (xb + 50 * ux - 50 * uy) - camx,
                zoom * (yb + 50 * ux + 50 * uy) - camy
            ),
            new PointF(
                zoom * (xb + 50 * ux + 50 * uy) - camx,
                zoom * (yb - 50 * ux + 50 * uy) - camy
            ),
            new PointF(
                zoom * (xb - 50 * ux + 50 * uy) - camx,
                zoom * (yb - 50 * ux - 50 * uy) - camy
            ),
            new PointF(
                zoom * (xb - 50 * ux - 50 * uy) - camx,
                zoom * (yb + 50 * ux - 50 * uy) - camy
            ),
        }, brush);

        g.DrawText(
            new RectangleF(
                zoom * (X - 25) - camx, 
                zoom * (Y - 25) - camy,
                zoom * 50, zoom * 50
            ),
            SystemFonts.MessageBoxFont,
            StringAlignment.Center,
            StringAlignment.Center,
            Brushes.Black, fit.ToString()
        );
    }

    const float acc = 600;
    const float fric = 0.1f;
    const float rot = 50;
    const float air = 0.001f;
    
    float[] collectInfo = null;
    public void TryCollect(PointF[] points)
    {
        if (points is null)
            return;

        if (collectInfo?.Length != points.Length)
            collectInfo = new float[points.Length];
        
        for (int i = 0; i < points.Length; i++)
        {
            var pt = points[i];
            var dx = pt.X - X;
            var dy = pt.Y - Y;
            var dist = dx * dx + dy * dy;

            var newPoints = dist > 300 * 300 ? 0 :
                (300 * 300 - dist) / (300 * 300);
            newPoints *= newPoints;

            if (collectInfo[i] < newPoints)
            {
                Points -= collectInfo[i];
                collectInfo[i] = newPoints;
                Points += newPoints;
            }
        }
    }
    public void Move(float dt)
    {
        var cdx = x - xb;
        var cdy = y - yb;
        var dist = MathF.Sqrt(cdx * cdx + cdy * cdy);
        cdx /= dist;
        cdy /= dist;

        var mod = MathF.Sqrt(vx * vx + vy * vy);
        var modb = MathF.Sqrt(vxb * vxb + vyb * vyb);
        
        ax = 0;
        ay = 0;

        axb = acc * cdx * power;
        ayb = acc * cdy * power;

        ax -= fric * cdy * MathF.Sign(vx);
        ay -= fric * cdx * MathF.Sign(vy);
        
        axb -= fric * cdy * MathF.Sign(vx);
        ayb -= fric * cdx * MathF.Sign(vy);
        
        if (mod > 1e-5)
        {
            ax -= air * vx * vx * vx / mod;
            ay -= air * vy * vy * vy / mod;
        }
        
        if (modb > 1e-5)
        {
            axb -= air * vxb * vxb * vxb / modb;
            ayb -= air * vyb * vyb * vyb / modb;
        }

        var dk = dist - springLen;
        var ak = springK * dk;

        ax -= ak * cdx;
        ay -= ak * cdy;
        axb += ak * cdx;
        ayb += ak * cdy;

        ax -= brake * vx;
        ay -= brake * vy;
        axb -= brake * vxb;
        ayb -= brake * vyb;

        vx += ax * dt;
        vy += ay * dt;

        vxb += axb * dt;
        vyb += ayb * dt;

        x += vx * dt;
        y += vy * dt;

        x -= rot * dir * cdy * dt;
        y += rot * dir * cdx * dt;

        xb += vxb * dt;
        yb += vyb * dt;
    }
    public void StartBrake()
        => brake = 1f;
    public void StopBrake()
        => brake = 0;
    public void StartAccelerate()
        => power = 1f;
    public void StopAccelerate()
        => power = 0f;    
    public void TurnLeft()
        => dir = -1f;
    public void TurnRight()
        => dir = 1f;
    public void StopTurn()
        => dir = 0f;
}