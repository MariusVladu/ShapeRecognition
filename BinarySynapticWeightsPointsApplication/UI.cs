﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BinarySynapticWeights.Contracts;
using BinarySynapticWeights.FeatureExtraction;
using BinarySynapticWeights.FeatureExtraction.Contracts;
using BinarySynapticWeights.FeatureExtraction.Entities;

namespace BinarySynapticWeightsPointsApplication
{
    public partial class UI : Form
    {
        private Graphics graphics;
        private Pen regularPen;
        private Brush significantPointBrush;
        private Brush centerPointBrush;
        private Pen rayPen;
        private Pen preprocessedSegmentsPen;
        private List<IStroke> strokes = new List<IStroke>();
        private IStroke currentStroke;
        private Point lastDrawnPoint;

        private IBSWTrainingAlgorithm bswTrainingAlgorithm;
        private IBSWNeuralNetwork bswNeuralNetwork;

        private Point centerOfGravityPoint;
        private List<Point> significantPointsAfterRayIntersection;
        private ShapeFeatures currentShapeFeatures;

        private static readonly CenterOfGravity CenterOfGravity = new CenterOfGravity();
        private static readonly RayIntersection RayIntersection = new RayIntersection(32);
        private static readonly FeatureExtraction FeatureExtraction = new FeatureExtraction();

        public UI()
        {
            InitializeComponent();

            graphics = drawingPictureBox.CreateGraphics();
            regularPen = new Pen(Color.Black, 2);
            significantPointBrush = (Brush)Brushes.Green;
            centerPointBrush = (Brush)Brushes.Red;
            rayPen = new Pen(Color.Blue, 1);
            preprocessedSegmentsPen = new Pen(Color.LightBlue, 2);
        }

        private void drawingPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentStroke = new Stroke(e.Location);
                lastDrawnPoint = e.Location;
            }
        }

        private void drawingPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point currentPoint = e.Location;
                graphics.DrawLine(regularPen, lastDrawnPoint, currentPoint);
                lastDrawnPoint = currentPoint;
                bool isNewSignificantPoint = currentStroke.AppendPoint(currentPoint);
                if (isNewSignificantPoint)
                {
                    Point lastSignificant = currentStroke.GetLastSignificantPoint();
                    graphics.FillRectangle(significantPointBrush, lastSignificant.X - 2, lastSignificant.Y - 2, 4, 4);
                }
            }
        }

        private void drawingPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentStroke.GetNumberOfPoints() > 1)
            {
                strokes.Add(currentStroke);
            }
        }

        private void ResetCanvasButton_Click(object sender, EventArgs e)
        {
            drawingPictureBox.Refresh();
            strokes = new List<IStroke>();

            acuteAnglesLabel.Text = "# Acute angles = ";
            rightAnglesLabel.Text = "# Right angles = ";
            wideAnglesLabel.Text = "# Wide angles = ";
            straightAnglesLabel.Text = "# Straight angles = ";
        }

        private void CenterOfGravityButton_Click(object sender, EventArgs e)
        {
            centerOfGravityPoint = CenterOfGravity.GetCenterOfGravity(strokes);

            graphics.FillRectangle(centerPointBrush, centerOfGravityPoint.X - 4, centerOfGravityPoint.Y - 4, 9, 9);
        }

        private void showRaysButton_Click(object sender, EventArgs e)
        {
            significantPointsAfterRayIntersection = RayIntersection.GetSignificanPointsAfterRayIntersection(strokes, centerOfGravityPoint);

            foreach (var point in significantPointsAfterRayIntersection)
            {
                graphics.DrawLine(rayPen, centerOfGravityPoint, point);
                DrawPoint(point, 9);
            }

            DrawSegmentsAfterRaysIntersection();
        }

        private void DrawSegmentsAfterRaysIntersection()
        {
            for (int i = 1; i < significantPointsAfterRayIntersection.Count; i++)
            {
                graphics.DrawLine(preprocessedSegmentsPen, significantPointsAfterRayIntersection[i - 1], significantPointsAfterRayIntersection[i]);
            }

            graphics.DrawLine(preprocessedSegmentsPen, significantPointsAfterRayIntersection.Last(), significantPointsAfterRayIntersection.First());
        }

        private void DrawPoint(Point point, int size)
        {
            graphics.FillRectangle(rayPen.Brush, (float)point.X - size / 2, (float)point.Y - size / 2, size, size);
        }

        private void ExtractFeaturesButton_Click(object sender, EventArgs e)
        {
            currentShapeFeatures = FeatureExtraction.GetShapeFeatures(significantPointsAfterRayIntersection);

            acuteAnglesLabel.Text += currentShapeFeatures.AcuteAngles;
            rightAnglesLabel.Text += currentShapeFeatures.RightAngles;
            wideAnglesLabel.Text += currentShapeFeatures.WideAngles;
            straightAnglesLabel.Text += currentShapeFeatures.StraightAngles;
        }

    }
}
