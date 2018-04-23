﻿using System;
using System.Windows.Forms;
using DevExpress.XtraMap;
using System.IO;

namespace CustomProvider {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            // Create a map control, set its dock style and add it to the form.
            MapControl map = new MapControl();
            map.Dock = DockStyle.Fill;
            this.Controls.Add(map);

            // Create a layer to load image tiles from a local map data provider.
            ImageTilesLayer imageTilesLayer = new ImageTilesLayer();
            map.Layers.Add(imageTilesLayer);
            imageTilesLayer.DataProvider = new LocalProvider();

        }

        public class LocalProvider : MapDataProviderBase {
            readonly SphericalMercatorProjection projection = new SphericalMercatorProjection();

            public LocalProvider() {
                TileSource = new LocalTileSource(this);
            }

            public override IProjection Projection {
                get {
                    return projection;
                }
            }
            public override MapSize GetMapSizeInPixels(double zoomLevel) {
                double imageSize;
                if (zoomLevel < 1.0)
                    imageSize = zoomLevel * LocalTileSource.tileSize * 2;
                imageSize = Math.Pow(2.0, zoomLevel) * LocalTileSource.tileSize;
                return new MapSize(imageSize, imageSize);
            }
        }

        public class LocalTileSource : MapTileSourceBase {
            public const int tileSize = 256;
            public const int maxZoomLevel = 1;

            public LocalTileSource(ICacheOptionsProvider cacheOptionsProvider) : base(512, 512, 256, 256, cacheOptionsProvider) { }

            public override Uri GetTileByZoomLevel(int zoomLevel, int tilePositionX, int tilePositionY) {
                if (zoomLevel == 1)
                    return new Uri(string.Format("file://" + Directory.GetCurrentDirectory() + "\\tile_{0}_{1}_{2}.png", zoomLevel, tilePositionX, tilePositionY));
                return null;
            }
        }

    }
}
