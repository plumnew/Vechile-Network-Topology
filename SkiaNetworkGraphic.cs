using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SkiaSharp;
using SkiaSharp.Views.WPF;

namespace DataFlowDesigner.SkiaNetworkGraphic
{
    public class SkiaNetworkGraphic: SKElement
    {
        double zoom = 1.0;
        List<SkiaNetworkNode> NetworkNodes = new List<SkiaNetworkNode>();
        List<SkiaBus> NetworkBus = new List<SkiaBus>();
        List<SkiaLink> Links = new List<SkiaLink>();
        SkiaBaseObject handleNode;


        public delegate void EditSelectObject(SkiaBaseObject baseObject);
        public event EditSelectObject editSelectObject;

        public ContextMenu GraphicMenu { set; get; } = new ContextMenu();
        MenuItem mode = new MenuItem { Header = "Draw Mode" };
        MenuItem addNode = new MenuItem() { Header = "Add Node" };
        MenuItem removeNode = new MenuItem() { Header = "Remove Node" };
        MenuItem addPort = new MenuItem() { Header = "Add Port" };
        MenuItem removePort = new MenuItem() { Header = "Remove Port" };
        MenuItem addBus = new MenuItem() { Header = "Add Bus" };
        MenuItem removeBus = new MenuItem() { Header = "Remove Bus" };
        public SkiaNetworkGraphic() : base()
        {
            base.Width = 640;
            base.Height = 480;
            base.PaintSurface += SkiaConnectGraphic_PaintSurface;
            base.MouseDown += SkiaConnectGraphic_MouseDown;
            base.MouseMove += SkiaConnectGraphic_MouseMove;
            base.MouseUp += SkiaConnectGraphic_MouseUp;

            NetworkNodes.Add(new SkiaNetworkNode(10, 10, 100, 40));
            addNode.Click += AddNode_Click;
            removeNode.Click += RemoveNode_Click; 
            addPort.Click += AddPort_Click;
            removePort.Click += RemovePort_Click;
            addBus.Click += AddBus_Click; 
            removeBus.Click += RemoveBus_Click;
            mode.Click += Mode_Click;

            GraphicMenu.Items.Add(mode);
            GraphicMenu.Items.Add(addBus);
            GraphicMenu.Items.Add(removeBus);
            GraphicMenu.Items.Add(addNode);
            GraphicMenu.Items.Add(removeNode);
            GraphicMenu.Items.Add(addPort);
            GraphicMenu.Items.Add(removePort);
            ContextMenu = GraphicMenu;
        }

        private void Mode_Click(object sender, RoutedEventArgs e)
        {
            mode.IsChecked = !mode.IsChecked;
        }

        private void RemoveBus_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddBus_Click(object sender, RoutedEventArgs e)
        {
            var mp = GraphicMenu.TranslatePoint(new Point(0, 0), this);
            NetworkBus.Add(new SkiaBus((float)mp.X, (float)mp.Y));
            this.InvalidateVisual();
        }

        private void RemovePort_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddPort_Click(object sender, RoutedEventArgs e)
        {
            var mp = GraphicMenu.TranslatePoint(new Point(0, 0), this);
            foreach (var node in NetworkNodes)
            {
                switch (node.HitTest((float)mp.X, (float)mp.Y))
                {
                    case HandleType.NODEEDIT:
                        node.AddPort((float)mp.X, (float)mp.Y);
                        this.InvalidateVisual();
                        return;
                    default:
                        continue;
                }
            }
        }

        private void RemoveNode_Click(object sender, RoutedEventArgs e)
        {
            NetworkNodes.RemoveAll(n => n.isSelected == true);
            this.InvalidateVisual();
        }

        private void AddNode_Click(object sender, RoutedEventArgs e)
        {
            
            var mp = GraphicMenu.TranslatePoint(new Point(0, 0) ,this);
            NetworkNodes.Add(new SkiaNetworkNode((float)mp.X, (float)mp.Y, 100, 40));
            this.InvalidateVisual();
        }

        private void SkiaConnectGraphic_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.MiddleButton == System.Windows.Input.MouseButtonState.Released && handleNode is SkiaNetworkNode)
            {
                Point mp = e.GetPosition(this);
                foreach (var node in NetworkNodes)
                {
                    switch (node.HitTest((float)mp.X, (float)mp.Y, true))
                    {
                        case HandleType.LINK:
                            {
                                if (((SkiaNetworkNode)handleNode).capedPort.Equals(node.capedPort))
                                    continue;
                                Links.Add(new SkiaLink(((SkiaNetworkNode)handleNode).capedPort, node.capedPort));
                                if (handleNode != null)
                                    handleNode.ReleaseHandle();
                                this.InvalidateVisual();
                            }
                            return;
                    }
                }
                foreach (var bus in NetworkBus)
                {
                    switch (bus.HitTest((float)mp.X, (float)mp.Y, true))
                    {
                        case HandleType.LINK:
                            {
                                Links.Add(new SkiaLink(((SkiaNetworkNode)handleNode).capedPort, bus));
                                if (handleNode != null)
                                    handleNode.ReleaseHandle();
                                this.InvalidateVisual();
                            }
                            return;
                    }
                }
            }
            if (handleNode!=null)
                handleNode.ReleaseHandle();
            this.InvalidateVisual();
        }

        private void SkiaConnectGraphic_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                Point mp = e.GetPosition(this);
                if (handleNode is SkiaNetworkNode)
                {
                    switch (handleNode.connectHandleType)
                    {
                        case HandleType.NODEMOV:
                            ((SkiaNetworkNode)handleNode).MoveNode((float)mp.X, (float)mp.Y);
                            this.InvalidateVisual();
                            break;
                        case HandleType.PORTMOV:
                            ((SkiaNetworkNode)handleNode).MovePort((float)mp.X, (float)mp.Y);
                            this.InvalidateVisual();
                            break;
                        default:
                            break;
                    }
                }
                else if(handleNode is SkiaBus)
                {
                    switch (handleNode.connectHandleType)
                    {
                        case HandleType.BUSMOV:
                            ((SkiaBus)handleNode).MoveBus((float)mp.X, (float)mp.Y);
                            this.InvalidateVisual();
                            break;
                        case HandleType.BUSSHAPE:
                            ((SkiaBus)handleNode).ReshapeBus((float)mp.X, (float)mp.Y);
                            this.InvalidateVisual();
                            break;
                        default:
                            break;
                    }
                }
            }
            if(e.MiddleButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                Point mp = e.GetPosition(this);
                if (handleNode is SkiaNetworkNode)
                {
                    switch (handleNode.connectHandleType)
                    {
                        case HandleType.LINK:
                            ((SkiaNetworkNode)handleNode).LinkShow((float)mp.X, (float)mp.Y);
                            this.InvalidateVisual();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void SkiaConnectGraphic_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                Point mp = e.GetPosition(this);
                foreach (var node in NetworkNodes)
                {
                    switch (node.HitTest((float)mp.X, (float)mp.Y))
                    {
                        case HandleType.NODEMOV:
                        case HandleType.PORTMOV:
                            handleNode = node;
                            return;
                        default:
                            continue;
                    }
                }

                foreach (var bus in NetworkBus)
                {
                    switch (bus.HitTest((float)mp.X, (float)mp.Y))
                    {
                        case HandleType.BUSMOV:
                        case HandleType.BUSSHAPE:
                            handleNode = bus;
                            return;
                        default:
                            continue;
                    }
                }
                return;
            }
            if(e.MiddleButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                Point mp = e.GetPosition(this);
                foreach (var node in NetworkNodes)
                {
                    switch (node.HitTest((float)mp.X, (float)mp.Y, true))
                    {
                        case HandleType.LINK:
                            handleNode = node;
                            return;
                        case HandleType.None:
                            continue;
                    }
                }
            }
        }

        private void SkiaConnectGraphic_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            // get the screen density for scaling
            var scale = (float)PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;
            var scaledSize = new SKSize(e.Info.Width / scale, e.Info.Height / scale);

            // handle the device screen density
            canvas.Scale(scale * (float)zoom);
            canvas.Clear(SKColors.Black);

            foreach (var node in NetworkNodes)
            {
                node.Render(canvas);
            }
            foreach(var bus in NetworkBus)
            {
                bus.Render(canvas);
            }
            foreach (var link in Links)
            {
                link.Render(canvas);
            }
        }
    }
}
