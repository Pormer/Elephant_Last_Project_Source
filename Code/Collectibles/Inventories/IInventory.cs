using System;
using System.Collections;
using System.Collections.Generic;
using Code.Collectibles.Core;
using UnityEngine;

namespace Code.Collectibles.Inventories
{
    public interface IInventory<T> where T : CollectibleDataSO
    {
        public IEnumerable<T> Items { get; }
        public bool Has(T item);
    }
}