using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataFlowDesigner.SkiaNetworkGraphic
{
    public enum ConnectType { BUS, E2E, NONE};
    public enum HandleType { NODEMOV, NODEEDIT, PORTMOV, BUSMOV, BUSSHAPE, LINK, LINKCAP, None }

    public enum PortPosition { Top, Bottom, Left, Right, UNKOWN};

    public class SkiaBaseObject
    {
        public string name { set; get; }
        public string guid { set; get; }
        public HandleType connectHandleType = HandleType.None;
        public bool isSelected = false;
        public SKPaint paint = new SKPaint();

        public virtual void ReleaseHandle()
        {
            isSelected = false;
            connectHandleType = HandleType.None;
        }
    }

    public class SkiaLink: SkiaBaseObject
    {
        public SkiaNetworkPort end0;
        public SkiaBaseObject end1;

        public ConnectType connectType { set; get; } = ConnectType.NONE;

        public SkiaLink(SkiaNetworkPort p0, SkiaBus px)
        {
            end0 = p0;
            end1 = px;
            connectType = ConnectType.BUS;
        }

        public SkiaLink(SkiaNetworkPort p0, SkiaNetworkPort px)
        {
            end0 = p0;
            end1 = px;
            connectType = ConnectType.E2E;
        }
        public void Render(SKCanvas canvas)
        {
            switch(connectType)
            {
                case ConnectType.E2E:
                    PortConnectPort(canvas);
                    return;
                case ConnectType.BUS:
                    PortConnectBus(canvas);
                    return;
                default:
                    return;
            }
        }

        void PortConnectPort(SKCanvas canvas)
        {
            var end = end1 as SkiaNetworkPort;
            if((end0.portPosition == PortPosition.Left && end.portPosition == PortPosition.Right) ||
                (end0.portPosition == PortPosition.Right && end.portPosition == PortPosition.Left))
            {
                paint.IsStroke = false;
                paint.StrokeWidth = 2;
                paint.Color = SKColors.Green;
                canvas.DrawCircle(end0.point, 3, paint);
                canvas.DrawCircle(end.point, 3, paint);
                canvas.DrawLine(end0.point, new SKPoint((end0.point.X + end.point.X) / 2, end0.point.Y), paint);
                canvas.DrawLine(end.point, new SKPoint((end0.point.X + end.point.X) / 2, end.point.Y), paint);
                canvas.DrawLine(new SKPoint((end0.point.X + end.point.X) / 2, end0.point.Y), new SKPoint((end0.point.X + end.point.X) / 2, end.point.Y), paint);
            }
            else if((end0.portPosition == PortPosition.Top && end.portPosition == PortPosition.Bottom) ||
                (end0.portPosition == PortPosition.Bottom && end.portPosition == PortPosition.Top))
            {
                paint.IsStroke = false;
                paint.StrokeWidth = 2;
                paint.Color = SKColors.Green;
                canvas.DrawCircle(end0.point, 3, paint);
                canvas.DrawCircle(end.point, 3, paint);
                canvas.DrawLine(end0.point, new SKPoint(end0.point.X, (end0.point.Y + end.point.Y) / 2), paint);
                canvas.DrawLine(end.point, new SKPoint(end.point.X, (end0.point.Y + end.point.Y) / 2), paint);
                canvas.DrawLine(new SKPoint(end0.point.X, (end0.point.Y + end.point.Y) / 2), new SKPoint(end.point.X, (end0.point.Y + end.point.Y) / 2), paint);
            }
            else if(end0.portPosition == PortPosition.Bottom && end.portPosition == PortPosition.Bottom)
            {
                paint.IsStroke = false;
                paint.Color = SKColors.Green;
                canvas.DrawCircle(end0.point, 3, paint);
                canvas.DrawCircle(end.point, 3, paint);
                float max_y = Math.Max(end0.point.Y, end.point.Y) + 10;

                canvas.DrawLine(end0.point, new SKPoint(end0.point.X, max_y), paint);
                canvas.DrawLine(end.point, new SKPoint(end.point.X, max_y), paint);
                canvas.DrawLine(new SKPoint(end0.point.X, max_y), new SKPoint(end.point.X, max_y), paint);
            }
            else if (end0.portPosition == PortPosition.Top && end.portPosition == PortPosition.Top)
            {
                paint.IsStroke = false;
                paint.StrokeWidth = 2;
                paint.Color = SKColors.Green;
                canvas.DrawCircle(end0.point, 3, paint);
                canvas.DrawCircle(end.point, 3, paint);
                float min_y = Math.Min(end0.point.Y, end.point.Y) - 10;

                canvas.DrawLine(end0.point, new SKPoint(end0.point.X, min_y), paint);
                canvas.DrawLine(end.point, new SKPoint(end.point.X, min_y), paint);
                canvas.DrawLine(new SKPoint(end0.point.X, min_y), new SKPoint(end.point.X, min_y), paint);
            }
            else if (end0.portPosition == PortPosition.Right && end.portPosition == PortPosition.Right)
            {
                paint.IsStroke = false;
                paint.StrokeWidth = 2;
                paint.Color = SKColors.Green;
                canvas.DrawCircle(end0.point, 3, paint);
                canvas.DrawCircle(end.point, 3, paint);
                float max_x = Math.Max(end0.point.X, end.point.X) + 10;

                canvas.DrawLine(end0.point, new SKPoint(max_x, end0.point.Y), paint);
                canvas.DrawLine(end.point, new SKPoint(max_x, end.point.Y), paint);
                canvas.DrawLine(new SKPoint(max_x, end0.point.Y), new SKPoint(max_x, end.point.Y), paint);
            }
            else if (end0.portPosition == PortPosition.Left && end.portPosition == PortPosition.Left)
            {
                paint.IsStroke = false;
                paint.StrokeWidth = 2;
                paint.Color = SKColors.Green;
                canvas.DrawCircle(end0.point, 3, paint);
                canvas.DrawCircle(end.point, 3, paint);
                float min_x = Math.Min(end0.point.X, end.point.X) - 10;

                canvas.DrawLine(end0.point, new SKPoint(min_x, end0.point.Y), paint);
                canvas.DrawLine(end.point, new SKPoint(min_x, end.point.Y), paint);
                canvas.DrawLine(new SKPoint(min_x, end0.point.Y), new SKPoint(min_x, end.point.Y), paint);
            }
            else if(end0.portPosition == PortPosition.Top || end0.portPosition == PortPosition.Bottom)
            {
                paint.IsStroke = false;
                paint.StrokeWidth = 2;
                paint.Color = SKColors.Green;
                canvas.DrawCircle(end0.point, 3, paint);
                canvas.DrawCircle(end.point, 3, paint);
                canvas.DrawLine(end0.point, new SKPoint(end0.point.X, end.point.Y), paint);
                canvas.DrawLine(end.point, new SKPoint(end0.point.X, end.point.Y), paint);
            }
            else if(end0.portPosition == PortPosition.Left || end0.portPosition == PortPosition.Right)
            {
                paint.IsStroke = false;
                paint.StrokeWidth = 2;
                paint.Color = SKColors.Green;
                canvas.DrawCircle(end0.point, 3, paint);
                canvas.DrawCircle(end.point, 3, paint);
                canvas.DrawLine(end0.point, new SKPoint(end.point.X, end0.point.Y), paint);
                canvas.DrawLine(end.point, new SKPoint(end.point.X, end0.point.Y), paint);
            }
            else
            {
                new NotImplementedException();
            }
        }

        void PortConnectBus(SKCanvas canvas)
        {
            var bus = end1 as SkiaBus;
            if(bus.end0.X == bus.end1.X) // Horizion
            {
                canvas.DrawCircle(end0.point, 3, bus.paint);
                canvas.DrawLine(end0.point, new SKPoint(bus.end0.X, end0.point.Y), bus.paint);
            }
            else //Vertical
            {
                canvas.DrawCircle(end0.point, 3, bus.paint);
                canvas.DrawLine(end0.point, new SKPoint(end0.point.X, bus.end0.Y), bus.paint);
            }
        }
        //HitMode HitTest(System.Windows.Point point);
    }

    public class SkiaBus: SkiaBaseObject
    {
        public SKPoint end0;
        public SKPoint end1;
        float hitoffsetX = 0, hitoffsetY = 0;
        int HandelPointIndex = -1;

        public SkiaBus(float x, float y)
        {
            end0.X = x;
            end0.Y = y;
            end1.X = x + 100;
            end1.Y = y;
        }
        public void Render(SKCanvas canvas)
        {
            if(isSelected)
                paint.Color = SKColors.Orange;
            else
                paint.Color = SKColors.Blue;
            paint.StrokeWidth = 2;
            paint.IsStroke = false;
            canvas.DrawLine(end0, end1, paint);
            canvas.DrawCircle(end0, 6, paint);
            canvas.DrawCircle(end1, 6, paint);
        }

        public HandleType HitTest(float x, float y, bool isLinkHandle = false)
        {
            if(isLinkHandle)
            {
                if (SKPoint.Distance(end0, new SKPoint(x, y)) + SKPoint.Distance(end1, new SKPoint(x, y)) < SKPoint.Distance(end0, end1) + 2)
                {
                    hitoffsetX = x - end0.X;
                    hitoffsetY = y - end0.Y;
                    isSelected = true;
                    connectHandleType = HandleType.BUSMOV;
                    return HandleType.LINK;
                }
                return HandleType.None;
            }
            isSelected = false;
            if (SKPoint.Distance(end0, new SKPoint(x, y)) < 6)
            {
                HandelPointIndex = 0;
                isSelected = true;
                connectHandleType = HandleType.BUSSHAPE;
                return HandleType.BUSSHAPE;
            }
            else if(SKPoint.Distance(end1, new SKPoint(x, y)) < 6)
            {
                HandelPointIndex = 1;
                isSelected = true;
                connectHandleType = HandleType.BUSSHAPE;
                return HandleType.BUSSHAPE;
            }
            else if(SKPoint.Distance(end0, new SKPoint(x, y)) + SKPoint.Distance(end1, new SKPoint(x, y)) < SKPoint.Distance(end0, end1) + 2)
            {
                hitoffsetX = x - end0.X;
                hitoffsetY = y - end0.Y;
                isSelected = true;
                connectHandleType = HandleType.BUSMOV;
                return HandleType.BUSMOV;
            }
            return HandleType.None;
        }
        
        public void ReshapeBus(float x, float y)
        {
            if(HandelPointIndex==1)
            {
                if(Math.Abs(x - end0.X) < Math.Abs(y - end0.Y))
                {
                    end1.X = end0.X;
                    end1.Y = y;
                }
                else
                {
                    end1.X = x;
                    end1.Y = end0.Y;
                }
            }
            else if(HandelPointIndex == 0)
            {
                if (Math.Abs(x - end1.X) < Math.Abs(y - end1.Y))
                {
                    end0.X = end1.X;
                    end0.Y = y;
                }
                else
                {
                    end0.X = x;
                    end0.Y = end1.Y;
                }
            }
        }

        public void MoveBus(float x, float y)
        {
            var offset_x = -end0.X + x - hitoffsetX;
            var offset_y = -end0.Y + y - hitoffsetY;
            end0.Offset(offset_x, offset_y);
            end1.Offset(offset_x, offset_y);
        }
    }

    public class SkiaNetworkPort : SkiaBaseObject, IEquatable<SkiaNetworkPort>
    {
        public ConnectType connectType { set; get; }
        public PortPosition portPosition = PortPosition.UNKOWN;

        float radius = 4;
        public SKPoint point;
        float temp_x = 0;
        float temp_y = 0;
        bool islinkshow = false;
        public SkiaNetworkPort(float x, float y, PortPosition position)
        {
            portPosition = position;
            point = new SKPoint(x, y);
        }
        public void Render(SKCanvas canvas)
        {
            if (isSelected)
            {
                paint.IsStroke = false;
                paint.Color = SKColors.Orange;
            }
            else
            {
                paint.IsStroke = false;
                paint.Color = SKColors.White;
            }
            canvas.DrawCircle(point, radius, paint);
            if (islinkshow)
            {
                paint.Color = SKColors.White;
                canvas.DrawLine(point, new SKPoint(temp_x, temp_y), paint);
            }
        }
        public HandleType HitTest(float x, float y, bool isLinkHandle = false)
        {
            isSelected = false;
            islinkshow = false;
            if (SKPoint.Distance(point, new SKPoint(x,y)) < radius)
            {
                if (!isLinkHandle)
                {
                    isSelected = true;
                    connectHandleType = HandleType.PORTMOV;
                    return HandleType.PORTMOV;
                }
                else
                {
                    connectHandleType = HandleType.LINK;
                    islinkshow = true;
                    return HandleType.LINK;
                }

            }
            return HandleType.None;
        }
        public void Transfer(float x, float y)
        {
            point.Offset(x, y);
        }

        public void SetPosition(float x, float y)
        {
            point.X = x;
            point.Y = y;
        }
        public void LinkShow(float x, float y)
        {
            temp_x = x;
            temp_y = y;
        }
        
        public override void ReleaseHandle()
        {
            connectHandleType = HandleType.None;
            isSelected = false;
            islinkshow = false;
        }

        public bool Equals(SkiaNetworkPort other)
        {
            return point.Equals(other.point);
        }
    }

    public class SkiaNetworkNode: SkiaBaseObject
    {
        public float pos_x { set; get; }
        public float pos_y { set; get; }
        public float length { set; get; }
        public float width { set; get; }

        List<SkiaNetworkPort> ports { set; get; } = new List<SkiaNetworkPort>();

        SKRect body0, body1;

        public float BounderyLineWidth = 9;

        float hitoffsetX = 0, hitoffsetY = 0;
        public float tempX = 0, tempY = 0, mouseX = 0, mouseY = 0;



        public SkiaNetworkPort capedPort;

        public SkiaNetworkNode(float x, float y, float width, float height)
        {
            pos_x = x;
            pos_y = y;
            body0 = new SKRect(x, y, x + width, y + height);
            body1 = new SKRect(x + BounderyLineWidth, y + BounderyLineWidth, x + width - BounderyLineWidth, y + height - BounderyLineWidth);
            guid = Guid.NewGuid().ToString();
        }

        public void AddPort(float x, float y)
        {
            PortPosition position = PortPosition.UNKOWN;
            if( y < (body1.Top + body0.Top) / 2 + BounderyLineWidth / 2)
            {
                y = (body1.Top + body0.Top) / 2;
                position = PortPosition.Top;
            }
            else if(y > (body1.Bottom + body0.Bottom) / 2 - BounderyLineWidth / 2)
            {
                y = (body1.Bottom + body0.Bottom) / 2;
                position = PortPosition.Bottom;
            }
            if (x < (body1.Left + body0.Left) / 2 + BounderyLineWidth / 2)
            {
                x = (body1.Left + body0.Left) / 2;
                position = PortPosition.Left;
            }
            else if(x > (body1.Right + body0.Right) / 2 - BounderyLineWidth / 2)
            {
                x = (body1.Right + body0.Right) / 2;
                position = PortPosition.Right;
            }
            ports.Add(new SkiaNetworkPort(x, y, position));
        }

        public void MovePort(float x, float y)
        {
            bool isYaxisMode = false;
            PortPosition position = PortPosition.UNKOWN;
            if (y < (body1.Top + body0.Top) / 2 + BounderyLineWidth / 2)
            {
                y = (body1.Top + body0.Top) / 2;
                position = PortPosition.Top;
            }
            else if (y > (body1.Bottom + body0.Bottom) / 2 - BounderyLineWidth / 2)
            {
                y = (body1.Bottom + body0.Bottom) / 2;
                position = PortPosition.Bottom;
            }
            else
            {
                isYaxisMode = true;
            }


            if (x < (body1.Left + body0.Left) / 2 + BounderyLineWidth / 2)
            {
                x = (body1.Left + body0.Left) / 2;
                position = PortPosition.Left;
            }
            else if (x > (body1.Right + body0.Right) / 2 - BounderyLineWidth / 2)
            {
                x = (body1.Right + body0.Right) / 2;
                position = PortPosition.Right;
            }
            else
            {
                if(isYaxisMode == true)
                {
                    if(x - (body1.Left + body0.Left) / 2 < (body1.Right + body0.Right) / 2 - x)
                    {
                        x = (body1.Left + body0.Left) / 2;
                        position = PortPosition.Left;
                    }
                    else
                    {
                        x = (body1.Right + body0.Right) / 2;
                        position = PortPosition.Right;
                    }
                }
            }
            capedPort.portPosition = position;
            capedPort.SetPosition(x, y);
        }

        public void Render(SKCanvas canvas) 
        {
            if(isSelected)
            {
                paint.IsStroke = false;
                paint.Color = SKColors.Orange;
            }
            else
            {
                paint.IsStroke = false;
                paint.Color = SKColors.DarkGray;
            }
            canvas.DrawRect(body0, paint);
            paint.IsStroke = false;
            paint.Color = SKColors.Black;
            canvas.DrawRect(body1, paint);
            foreach (var p in ports)
            {
                p.Render(canvas);
            }
        }
        public HandleType HitTest(float x, float y, bool isLinkHandle=false)
        {

            if (body1.Contains(x, y))
            {
                hitoffsetX = x - body1.Left;
                hitoffsetY = y - body1.Top;
                connectHandleType = HandleType.NODEMOV;
                isSelected = true;
                return HandleType.NODEMOV;
            }
            if (body0.Contains(x, y))
            {
                
                isSelected = false;

                foreach(var p in ports)
                {
                    if (p.HitTest(x, y, isLinkHandle) != HandleType.None)
                    {
                        connectHandleType = p.connectHandleType;
                        capedPort = p;
                        return connectHandleType;
                    }
                }
                connectHandleType = HandleType.NODEEDIT;
                return HandleType.NODEEDIT;
            }
            return HandleType.None;
        }

        public void MoveNode(float x, float y)
        {
            var offset_x = -pos_x + x - hitoffsetX;
            var offset_y = -pos_y + y - hitoffsetY;
            body0.Offset(offset_x, offset_y);
            body1.Offset(offset_x, offset_y);
            foreach(var p in ports)
            {
                p.Transfer(offset_x, offset_y);
            }

            pos_x = x - hitoffsetX;
            pos_y = y - hitoffsetY;
        }
        
        public void LinkShow(float x, float y)
        {
            capedPort.LinkShow(x, y);
        }

        public override void ReleaseHandle()
        {
            connectHandleType = HandleType.None;
            isSelected = false;
            if (capedPort != null)
                capedPort.ReleaseHandle();
        }
        //XmlNode Serilize(XmlDocument document);
    }

}
