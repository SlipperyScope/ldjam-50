using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.TileSet;

namespace ldjam50.Refactor.Utils.Tiles
{
    /// <summary>
    /// Information about a tilemap tile
    /// </summary>
    public record Tile
    {
        public Int32 ID { get; private set; }
        public String Name { get; private set; }
        public Texture NormalMap { get; private set; }
        public Vector2 TextureOffset { get; private set; }
        public ShaderMaterial Material { get; private set; }
        public Color Modulate { get; private set; }
        public TileMode Mode { get; private set; }
        public BitmaskMode BitMaskMode { get; private set; }
        public Vector2 SubtileSize { get; private set; }
        public Int32 SubtileSpacing { get; private set; }
        public Vector2 OccluderOffset { get; private set; }
        public Vector2 NavigationOffset { get; private set; }
        public Vector2 ShapeOffset { get; private set; }
        public Transform2D ShapeTransform { get; private set; }
        public Int32 ZIndex { get; private set; }

        public Tile() { }
        public Tile(TileSet set, Int32 id)
        {
            ID = id;
            Name = set.TileGetName(id);
            NormalMap = set.TileGetNormalMap(id);
            TextureOffset = set.TileGetTextureOffset(id);
            Material = set.TileGetMaterial(id);
            Modulate = set.TileGetModulate(id);
            Mode = set.TileGetTileMode(id);
            BitMaskMode = set.AutotileGetBitmaskMode(id);
            SubtileSize = set.AutotileGetSize(id);
            SubtileSpacing = set.AutotileGetSpacing(id);
            OccluderOffset = set.TileGetOccluderOffset(id);
            NavigationOffset = set.TileGetNavigationPolygonOffset(id);
            ShapeOffset = set.TileGetShapeOffset(id, 0);
            ShapeTransform = set.TileGetShapeTransform(id, 0);
            ZIndex = set.TileGetZIndex(id);
        }
    }
}
