using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EditableDictionary<TK, TV> : SerializableDictionary<TK, TV>, IDictionary<TK, TV>
{
    private int v = 0;
    new public TV this[TK key] { get => Get(key); set => Set(key, value); }

    private void Set(TK key, TV value) {
        if (key == null) { throw new ArgumentNullException(); }
        if (IsReadOnly) { throw new NotSupportedException(); }
        Set(new Pair(key, value));
    }


    private TV Get(TK key) {
        if (key == null) { throw new ArgumentNullException(); }
        if (IsReadOnly) { throw new NotSupportedException(); }
        return base[key];
    }

    new public ICollection<TK> Keys => new List<TK>(base.Keys); //TODO there must be a better way of doing this, this seems hefty on overhead

    new public ICollection<TV> Values => new List<TV>(base.Values); //TODO there must be a better way of doing this, this seems hefty on overhead

    new public int Count => base.Count;

    //I have no fucking clue why this exists
    public bool IsReadOnly => false;

    public void Add(TK key, TV value) {
        if (!IsReadOnly) {
            if (key == null) { throw new ArgumentNullException(); }
            if (ContainsKey(key)) {
                throw new ArgumentException("Dictionary already contains key " + key);
            } else {
                Add(new Pair(key, value));
                v++;
            }
        } else {
            throw new NotSupportedException("Dictionary is read-only");
        }
    }

    public void Add(KeyValuePair<TK, TV> item) {
        Add(item.Key, item.Value);
    }

    new public void Clear() {
        if (!IsReadOnly) {
            base.Clear();
            v++;
        } else {
            throw new NotSupportedException("Dictionary is read-only");
        }
    }



    public bool Contains(KeyValuePair<TK, TV> item) {
        if (ContainsKey(item.Key)) {
            return EqualityComparer<TV>.Default.Equals(Get(item.Key), item.Value);
        } else {
            return false;
        }
    }


    new public bool ContainsKey(TK key) {
        if (key == null) { throw new ArgumentNullException(); }
        return base.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex) {

        if (array == null) {
            throw new ArgumentNullException("Array is null");
        }


        if (arrayIndex < 0 || arrayIndex > array.Length) {
            throw new ArgumentOutOfRangeException("Array index is " + arrayIndex + " out of range of the range " + array.Length);
        }

        if (array.Length - arrayIndex < Count) {
            throw new ArgumentException("Dictionary is too large to fit into array," + Count + " > " + (array.Length - arrayIndex));
        }

        ICollection<TK> keys = this.Keys;
        int idx = arrayIndex;
        foreach (TK k in keys) {
            array[idx] = new KeyValuePair<TK, TV>(k, Get(k));
            idx++;
        }
    }


    public IEnumerator<KeyValuePair<TK, TV>> GetEnumeratorObject() {
        return new Enumerator(this);
    }


    new public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() {
        return GetEnumeratorObject();
    }

    new public bool Remove(TK key) {
        if (key == null) { throw new ArgumentNullException(); }
        if (IsReadOnly) { throw new NotSupportedException(); }
        if (!ContainsKey(key)) { return false; }
        v++;
        return base.Remove(key);
    }

    public bool Remove(KeyValuePair<TK, TV> item) {
        if (item.Key == null) { throw new ArgumentNullException(); }
        if (IsReadOnly) { throw new NotSupportedException(); }
        if (!ContainsKey(item.Key)) { return false; }
        TV val = Get(item.Key);
        if (item.Value.Equals(val)) {
            v++;
            return Remove(item.Key);

        }

        return false;
    }

    new public bool TryGetValue(TK key, out TV value) {
        if (key == null) { throw new ArgumentNullException(); }
        return base.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumeratorObject();
    }


    public struct Enumerator : IEnumerator<KeyValuePair<TK, TV>>
    {
        private readonly EditableDictionary<TK, TV> dictonary;
        private int version;
        private KeyValuePair<TK, TV> current;
        private IEnumerator<TK> keys;

        public KeyValuePair<TK, TV> Current {
            get { return current; }
        }

        internal Enumerator(EditableDictionary<TK, TV> dictionary) {
            dictonary = dictionary;
            version = dictionary.v;
            current = default(KeyValuePair<TK, TV>);
            keys = dictionary.Keys.GetEnumerator();
        }

        public bool MoveNext() {
            if (version != dictonary.v)
                throw new InvalidOperationException(string.Format("Enumerator version {0} != Dictionary version {1}", version, dictonary.v));

            bool moved = keys.MoveNext();
            if (!moved) {
                current = default(KeyValuePair<TK, TV>);
                return false;
            } else {
                TK key = keys.Current;
                current = new KeyValuePair<TK, TV>(key, dictonary[key]);
                return true;
            }
        }

        void IEnumerator.Reset() {
            if (version != dictonary.v)
                throw new InvalidOperationException(string.Format("Enumerator version {0} != Dictionary version {1}", version, dictonary.v));

            current = default(KeyValuePair<TK, TV>);
            keys.Reset();
        }

        object IEnumerator.Current {
            get { return Current; }
        }

        public void Dispose() {
        }
    }
}

