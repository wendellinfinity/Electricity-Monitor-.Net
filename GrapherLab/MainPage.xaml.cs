using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GrapherLab {
     public partial class MainPage : UserControl {
          Ellipse _pointer;
          DispatcherTimer _graphTimer;
          double _step = -10, _clip = -10, _feedValue = 0, _bottomOffset = 10, _rightOffset = 10;
          int _interval = 500;
          bool _newFeedPoint = true;
          Point _zeroPoint, _feedPoint, _lastPoint;
          string _serviceURL = @"http://localhost/ElectricityMonitor/PushSensorDataService.svc/sensor/{0}/pop";
          public MainPage() {
               InitializeComponent();

               // add the elements here
               this._pointer = new Ellipse();
               this._pointer.Width = 30;
               this._pointer.Height = 30;
               this._pointer.Opacity = 0.25f;
               this._pointer.Fill = new SolidColorBrush(Colors.Blue);
               this._pointer.Tag = Guid.NewGuid().ToString();

               // initialize the zero point
               this._zeroPoint = new Point(this.drawArea.Width - this._rightOffset, this.drawArea.Height - this._bottomOffset);
               this._lastPoint = this._zeroPoint;
               this._feedPoint = this._zeroPoint;
               // set the clip
               this.drawArea.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, this.drawArea.Width, this.drawArea.Height) };
               // start the rendering
               this._graphTimer = new DispatcherTimer();
               this._graphTimer.Interval = new TimeSpan(0, 0, 0, 0, this._interval);
               this.drawArea.Children.Add(this._pointer);
               this.drawArea.MouseMove += new MouseEventHandler(delegate(object sender, MouseEventArgs margs) {
                    // move the cursor on mouseover
                    Point m = margs.GetPosition(this.drawArea);
                    Canvas.SetLeft(this._pointer, m.X - this._pointer.Width / 2);
                    Canvas.SetTop(this._pointer, m.Y - this._pointer.Width / 2);
                    // display X,Y in textbox
                    coordinates.Text = string.Format("X: {0}; Y: {1}", (int)m.X, (int)m.Y);
               });
               // perform the necessary actions per tick
               this._graphTimer.Tick += new EventHandler(delegate(object sender, EventArgs e) {
                    this.TranslateGraph(); // translate other points
                    this.AddPoint(); // add the new point
               });
               this._graphTimer.Start();
               this.StartRetrievePoint("8000");
          }

          private void StartRetrievePoint(string node) {
               HttpWebRequest request;
               request = (HttpWebRequest)HttpWebRequest.Create(string.Format(this._serviceURL, node));
               request.Headers["Authorization"] = "Basic w.a.capili";
               request.Method = "GET";
               request.BeginGetResponse(new AsyncCallback(CreatePoint), request);
          }

          private void CreatePoint(IAsyncResult asynchronousResult) {
               HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
               HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
               string soapResponse;
               using (System.IO.StreamReader read = new System.IO.StreamReader(response.GetResponseStream())) {
                    soapResponse = read.ReadToEnd();
               }
               Domain.SensorData sd = Domain.SensorData.ReadSensorData(soapResponse);
               this._feedValue = sd.PowerLevel;
               this.drawArea.Dispatcher.BeginInvoke(new Action(delegate() {
                    this._feedPoint = new Point(this._zeroPoint.X, this.drawArea.Height - this._feedValue - this._bottomOffset);
               }));
               this._newFeedPoint = true;
               this.StartRetrievePoint(sd.NodeId.ToString());
          }

          /// <summary>
          /// Adds a point to the graph
          /// </summary>
          private void AddPoint() {
               // line to demo
               Line liner = new Line();
               liner.X1 = this._lastPoint.X;
               liner.Y1 = this._lastPoint.Y;
               liner.X2 = this._feedPoint.X;
               liner.Y2 = this._feedPoint.Y;
               liner.Stroke = new SolidColorBrush(Colors.Blue);
               liner.Fill = new SolidColorBrush(Colors.Blue);
               liner.StrokeThickness = 2;
               //this.DebugPoint(this._lastPoint.X, this._lastPoint.Y, Colors.Red);
               //this.DebugPoint(this._feedPoint.X, this._feedPoint.Y, Colors.Magenta);
               this.drawArea.Children.Add(liner);
               this._lastPoint.X = this._feedPoint.X;
               this._lastPoint.Y = this._feedPoint.Y;
               if (this._newFeedPoint) {
                    TextBlock feedValue = new TextBlock();
                    feedValue.Text = this._feedValue.ToString();
                    this.drawArea.Children.Add(feedValue);
                    Canvas.SetLeft(feedValue, this._feedPoint.X);
                    Canvas.SetTop(feedValue, this._feedPoint.Y - 13);
                    this._newFeedPoint = false;
                    Ellipse pt = new Ellipse();
                    pt.Fill = new SolidColorBrush(Colors.Blue);
                    pt.Stroke = new SolidColorBrush(Colors.Blue);
                    pt.Width = 5;
                    pt.Height = 5;
                    this.drawArea.Children.Add(pt);
                    Canvas.SetLeft(pt, this._feedPoint.X - pt.Width / 2);
                    Canvas.SetTop(pt, this._feedPoint.Y - pt.Height / 2);
               }
          }

          private void DebugPoint(double X, double Y, Color pcolor) {
               Ellipse pt = new Ellipse();
               pt.Fill = new SolidColorBrush(pcolor);
               pt.Stroke = new SolidColorBrush(pcolor);
               pt.Width = 5;
               pt.Height = 5;
               this.drawArea.Children.Add(pt);
               Canvas.SetLeft(pt, X - pt.Width / 2);
               Canvas.SetTop(pt, Y - pt.Height / 2);
          }

          /// <summary>
          /// Translate the lines on the graph
          /// </summary>
          private void TranslateGraph() {
               // for the UIElements
               IList<string> cleanElements;
               double width, right;
               System.Reflection.PropertyInfo widthP;
               Line graph;
               cleanElements = new List<string>();
               foreach (UIElement el in this.drawArea.Children) {
                    if (el != this._pointer) {
                         right = 0;
                         // if the object is not a line then get width, if it exists
                         if (el.GetType() != typeof(Line)) {
                              widthP = el.GetType().GetProperty("Width");
                              if (widthP != null) {
                                   width = (double)el.GetType().GetProperty("Width").GetValue(el, null);
                                   right = Canvas.GetLeft(el) + width;
                              }
                              Canvas.SetLeft(el, Canvas.GetLeft(el) + this._step);
                         }
                         // if the object is a line then set endpoint X1 and X2
                         if (el.GetType() == typeof(Line)) {
                              graph = (Line)el;
                              graph.X1 += this._step;
                              graph.X2 += this._step;
                              right = graph.X2;
                         }
                         // then clean up those outside of bounds
                         if (right < this._clip) {
                              //cleanElements.Add(el.); // ? Identifier yet tbd
                         }
                    }
               }
               // not yet working...
               foreach (string el in cleanElements) {
                    //this.drawArea.Children.Remove((UIElement)this.drawArea.FindName(el));
               }
               this._lastPoint.X += this._step;
               // end UI elements
          }

          private void newPoint_Click(object sender, RoutedEventArgs e) {
               this._feedValue = double.Parse(this.newYCoord.Text);
               this._feedPoint = new Point(this._zeroPoint.X, this.drawArea.Height - this._feedValue - this._bottomOffset);
               this._newFeedPoint = true;
          }

     }
}

