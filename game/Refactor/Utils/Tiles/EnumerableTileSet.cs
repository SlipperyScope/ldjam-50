using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Utils.Tiles
{
    /// <summary>
    /// A TileSet that is also a readonly collection of it's contained tiles
    /// </summary>
    public class EnumerableTileSet : TileSet, IEnumerable<Tile>//, ICollection<Tile>, IList<Tile>, IReadOnlyCollection<Tile>, IReadOnlyList<Tile>, IList
    {
        public ReadOnlyCollection<Tile> Tiles => _Tiles ??= GetTilesIds().ToList<Int32>().Select(id => new Tile(this, id)).ToList().AsReadOnly();
        private ReadOnlyCollection<Tile> _Tiles;

        public static implicit operator ReadOnlyCollection<Tile>(EnumerableTileSet set) => set.Tiles;
        public static implicit operator List<Tile>(EnumerableTileSet set) => set.Tiles.ToList();

        //public Tile this[Int32 index]
        //{
        //    get => (Tiles as IList<Tile>)[index];
        //    set => (Tiles as IList<Tile>)[index] = value;
        //}

        //System.Object IList.this[Int32 index]
        //{
        //    get => (Tiles as IList)[index];
        //    set => (Tiles as IList)[index] = value;
        //}

        //public Int32 Count => Tiles.Count;

        //public Boolean IsReadOnly => (Tiles as ICollection<Tile>).IsReadOnly;

        //public Boolean IsFixedSize => (Tiles as IList).IsFixedSize;

        //public System.Object SyncRoot => (Tiles as ICollection).SyncRoot;

        //public Boolean IsSynchronized => (Tiles as ICollection).IsSynchronized;

        //public void Add(Tile item) => (Tiles as ICollection<Tile>).Add(item);

        //public Int32 Add(System.Object value) => (Tiles as IList).Add(value);

        //public Boolean Contains(Tile item) => Tiles.Contains(item);

        //public Boolean Contains(System.Object value) => Tiles.Contains(value);

        //public void CopyTo(Tile[] array, Int32 arrayIndex) => Tiles.CopyTo(array, arrayIndex);

        //public void CopyTo(Array array, Int32 index) => Tiles.CopyTo(array as Tile[], index);
        ////{
        ////    if (array is null) throw new ArgumentNullException(nameof(array));
        ////    Tile[] tileArray = array as Tile[];
        ////    if (tileArray is null) throw new ArgumentException();
        ////    ((ICollection<Tile>)this).CopyTo(tileArray, index);
        ////}

        public IEnumerator<Tile> GetEnumerator() => Tiles.GetEnumerator();

        //public Int32 IndexOf(Tile item) => Tiles.IndexOf(item);

        //public Int32 IndexOf(System.Object value) => (Tiles as IList).IndexOf(value);

        //public void Insert(Int32 index, Tile item) => (Tiles as IList<Tile>).Insert(index, item);

        //public void Insert(Int32 index, System.Object value) => (Tiles as IList).Insert(index, value);

        //public Boolean Remove(Tile item) => (Tiles as ICollection<Tile>).Remove(item);

        //public void Remove(System.Object value) => (Tiles as IList).Remove(value);

        //public void RemoveAt(Int32 index) => (Tiles as IList<Tile>).RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => Tiles.GetEnumerator();
    }
}
