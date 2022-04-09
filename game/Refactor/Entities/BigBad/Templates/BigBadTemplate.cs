using Godot;
using ldjam50.Refactor.Utils;
using ldjam50.Refactor.Utils.Tiles;
using System;

namespace ldjam50.Refactor.Entities.BigBad.Templates
{
    /// <summary>
    /// Big bad configuration data
    /// </summary>
    public class BigBadTemplate : TileMap
    {
        /// <summary>
        /// Speed in m/s
        /// </summary>
        [Export]
        public Single Speed { get; private set; } = 10f;

        new public EnumerableTileSet TileSet => base.TileSet as EnumerableTileSet ?? throw new InvalidNodeCastException($"{base.TileSet.ResourceName} is not {nameof(EnumerableTileSet)}");
    }
}