using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MappaMentis.Domain.Entities;

namespace MappaMentis.Wpf.Views;

/// <summary>
/// UserControl for displaying a mind map in a radial layout.
/// The mind map title is shown in the center with nodes distributed evenly around it.
/// </summary>
public partial class MindMapRadialView : UserControl
{
    private const double NodeRadius = 60;
    private const double CircleRadius = 300;
    private const double LineStrokeWidth = 2;

    public MindMapRadialView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Renders a mind map on the canvas in a radial layout.
    /// </summary>
    public void RenderMindMap(MindMap mindMap)
    {
        if (mindMap == null)
            return;

        MindMapCanvas.Children.Clear();

        // Get canvas center
        double centerX = MindMapCanvas.ActualWidth / 2;
        double centerY = MindMapCanvas.ActualHeight / 2;

        // If canvas hasn't been measured yet, use default size
        if (double.IsNaN(centerX) || centerX == 0)
            centerX = 600;
        if (double.IsNaN(centerY) || centerY == 0)
            centerY = 400;

        var nodeCount = mindMap.Nodes.Count;

        // Draw links first (so they appear behind nodes)
        DrawLinks(mindMap, centerX, centerY, nodeCount);

        // Draw title in the center
        DrawCenterTitle(mindMap.Title, centerX, centerY);

        // Draw nodes in a circle around the center
        DrawRadialNodes(mindMap, centerX, centerY, nodeCount);
    }

    /// <summary>
    /// Draws the mind map title in the center of the canvas.
    /// </summary>
    private void DrawCenterTitle(string title, double centerX, double centerY)
    {
        // Create a circle background for the title
        Ellipse circle = new Ellipse
        {
            Width = NodeRadius * 2,
            Height = NodeRadius * 2,
            Fill = new SolidColorBrush(Color.FromRgb(100, 149, 237)), // Cornflower blue
            Stroke = new SolidColorBrush(Color.FromRgb(65, 105, 225)), // Royal blue
            StrokeThickness = 2
        };

        Canvas.SetLeft(circle, centerX - NodeRadius);
        Canvas.SetTop(circle, centerY - NodeRadius);
        Canvas.SetZIndex(circle, 5);

        MindMapCanvas.Children.Add(circle);

        // Create a Grid container to center the text
        Grid textContainer = new Grid
        {
            Width = NodeRadius * 2 - 10,
            Height = NodeRadius * 2 - 10
        };

        // Create text block for the title
        TextBlock titleText = new TextBlock
        {
            Text = title,
            FontSize = 14,
            FontWeight = FontWeights.Bold,
            Foreground = new SolidColorBrush(Colors.White),
            TextAlignment = TextAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        textContainer.Children.Add(titleText);
        Canvas.SetLeft(textContainer, centerX - (NodeRadius - 5));
        Canvas.SetTop(textContainer, centerY - (NodeRadius - 5));
        Canvas.SetZIndex(textContainer, 6);

        MindMapCanvas.Children.Add(textContainer);
    }

    /// <summary>
    /// Draws nodes in a circular pattern around the center.
    /// </summary>
    private void DrawRadialNodes(MindMap mindMap, double centerX, double centerY, int nodeCount)
    {
        if (nodeCount == 0)
            return;

        var nodes = mindMap.Nodes.ToList();

        for (int i = 0; i < nodeCount; i++)
        {
            // Calculate angle for even distribution
            double angle = (360.0 / nodeCount) * i;
            double radians = angle * Math.PI / 180.0;

            // Calculate node position
            double nodeX = centerX + CircleRadius * Math.Cos(radians);
            double nodeY = centerY + CircleRadius * Math.Sin(radians);

            // Draw connecting line from center to node
            DrawConnectionLine(centerX, centerY, nodeX, nodeY);

            // Draw the node
            DrawNode(nodes[i], nodeX, nodeY);
        }
    }

    /// <summary>
    /// Draws a line connecting the center to a node.
    /// </summary>
    private void DrawConnectionLine(double fromX, double fromY, double toX, double toY)
    {
        Line line = new Line
        {
            X1 = fromX,
            Y1 = fromY,
            X2 = toX,
            Y2 = toY,
            Stroke = new SolidColorBrush(Color.FromRgb(169, 169, 169)), // Dark gray
            StrokeThickness = LineStrokeWidth,
        };

        Canvas.SetZIndex(line, 1);
        MindMapCanvas.Children.Add(line);
    }

    /// <summary>
    /// Draws a single node on the canvas.
    /// </summary>
    private void DrawNode(MindNode node, double posX, double posY)
    {
        // Parse the node's color
        Color nodeColor = ParseColorFromString(node.Color);

        // Create circle for the node
        Ellipse nodeCircle = new Ellipse
        {
            Width = NodeRadius,
            Height = NodeRadius,
            Fill = new SolidColorBrush(nodeColor),
            Stroke = new SolidColorBrush(Colors.White),
            StrokeThickness = 2
        };

        Canvas.SetLeft(nodeCircle, posX - NodeRadius / 2);
        Canvas.SetTop(nodeCircle, posY - NodeRadius / 2);
        Canvas.SetZIndex(nodeCircle, 3);

        MindMapCanvas.Children.Add(nodeCircle);

        // Create text block for the node content
        TextBlock nodeText = new TextBlock
        {
            Text = node.Content,
            FontSize = 11,
            FontWeight = FontWeights.SemiBold,
            Foreground = new SolidColorBrush(Colors.White),
            TextAlignment = TextAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            Width = NodeRadius - 8,
            Height = NodeRadius - 8
        };

        Canvas.SetLeft(nodeText, posX - (NodeRadius - 8) / 2);
        Canvas.SetTop(nodeText, posY - (NodeRadius - 8) / 2);
        Canvas.SetZIndex(nodeText, 4);

        MindMapCanvas.Children.Add(nodeText);
    }

    /// <summary>
    /// Draws lines connecting linked nodes using curved paths through the center.
    /// </summary>
    private void DrawLinks(MindMap mindMap, double centerX, double centerY, int nodeCount)
    {
        if (nodeCount == 0 || mindMap.Links.Count == 0)
            return;

        var nodeList = mindMap.Nodes.ToList();
        var nodePositions = new Dictionary<Guid, (double x, double y)>();

        // Calculate positions for all nodes
        for (int i = 0; i < nodeCount; i++)
        {
            double angle = (360.0 / nodeCount) * i;
            double radians = angle * Math.PI / 180.0;
            double nodeX = centerX + CircleRadius * Math.Cos(radians);
            double nodeY = centerY + CircleRadius * Math.Sin(radians);
            nodePositions[nodeList[i].Id] = (nodeX, nodeY);
        }

        // Draw links between nodes
        foreach (var link in mindMap.Links)
        {
            if (nodePositions.TryGetValue(link.SourceNodeId, out var sourcePos) &&
                nodePositions.TryGetValue(link.TargetNodeId, out var targetPos))
            {
                // Create a curved path through the center
                PathGeometry pathGeometry = new PathGeometry();
                PathFigure pathFigure = new PathFigure
                {
                    StartPoint = new Point(sourcePos.x, sourcePos.y),
                    IsFilled = false
                };

                // Create a quadratic bezier curve that passes through the center
                QuadraticBezierSegment curve = new QuadraticBezierSegment
                {
                    Point1 = new Point(centerX, centerY), // Control point at center
                    Point2 = new Point(targetPos.x, targetPos.y)
                };

                pathFigure.Segments.Add(curve);
                pathGeometry.Figures.Add(pathFigure);

                // Create path shape
                Path linkPath = new Path
                {
                    Data = pathGeometry,
                    Stroke = new SolidColorBrush(ParseColorFromString(link.Color)),
                    StrokeThickness = LineStrokeWidth,
                    StrokeDashArray = link.LineStyle == "dashed" ? new DoubleCollection { 5, 5 } : null,
                    StrokeLineJoin = PenLineJoin.Round
                };

                Canvas.SetZIndex(linkPath, 2);
                MindMapCanvas.Children.Add(linkPath);
            }
        }
    }

    /// <summary>
    /// Parses a color string (hex format or named color) to a Color object.
    /// </summary>
    private Color ParseColorFromString(string colorString)
    {
        if (string.IsNullOrWhiteSpace(colorString))
            return Colors.Gray;

        try
        {
            // Handle hex colors like "#FFFFFF"
            if (colorString.StartsWith("#"))
            {
                return (Color)ColorConverter.ConvertFromString(colorString);
            }

            // Handle named colors
            return (Color)ColorConverter.ConvertFromString(colorString);
        }
        catch
        {
            return Colors.Gray;
        }
    }
}
