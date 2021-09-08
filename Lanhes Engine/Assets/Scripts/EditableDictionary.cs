using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EditableDictionary<TK, TV> : IDictionary<TK, TV>
{
    [Serializable]
    public class Pair<TK2, TV2>
    {
        [SerializeField]
        public TK2 key;
        [SerializeField]
        public TV2 value;
        public Pair(TK2 key, TV2 value) {
            this.key = key;
            this.value = value;
        }
    }



    [SerializeField]
    private List<Pair<TK, TV>> pairs = new List<Pair<TK, TV>>();


    public TV this[TK key] { get => Get(key); set => Set(key, value); }

    private void Set(TK key, TV value) {
        if (key == null) { throw new ArgumentNullException(); }
        if (IsReadOnly) { throw new NotSupportedException(); }
        Pair<TK, TV> pair;
        bool found = FindPair(key, out pair);
        if (!found) { throw new ArgumentException("argument " + key + "not found"); }
        pair.value = value;
        version++;
    }


    private TV Get(TK key) {
        if (key == null) { throw new ArgumentNullException(); }
        if (IsReadOnly) { throw new NotSupportedException(); }
        Pair<TK, TV> pair;
        bool found = FindPair(key, out pair);
        if (!found) { throw new ArgumentException("argument " + key + "not found"); }
        return pair.value;
    }

    public ICollection<TK> Keys => new List<TK>(pairs.ConvertAll(x => x.key));

    public ICollection<TV> Values => new List<TV>(pairs.ConvertAll(x => x.value));

    public int Count => pairs.Count;

    //I have no fucking clue why this exists
    public bool IsReadOnly => false;

    private int version = 0;

    public void Add(TK key, TV value) {
        if (!IsReadOnly) {
            if (key == null) { throw new ArgumentNullException(); }
            if (!ContainsKey(key)) {
                throw new ArgumentException("Dictionary already contains key " + key);
            } else {
                pairs.Add(new Pair<TK, TV>(key, value));
                version++;
            }
        } else {
            throw new NotSupportedException("Dictionary is read-only");
        }
    }

    public void Add(KeyValuePair<TK, TV> item) {
        Add(item.Key, item.Value);
    }

    public void Clear() {
        if (!IsReadOnly) {
            pairs.Clear();
            version++;
        } else {
            throw new NotSupportedException("Dictionary is read-only");
        }
    }



    public bool Contains(KeyValuePair<TK, TV> item) {
        Pair<TK, TV> pair;
        bool found = FindPair(item.Key,out pair);
        if (found) {
            return EqualityComparer<TV>.Default.Equals(pair.value, item.Value);
        } else {
            return false;
        }
    }

    private int FindIndex(TK key) {
        if (!ContainsKey(key)) { throw new KeyNotFoundException(); }
        return pairs.FindIndex(x => x.key.Equals(key));
    }

    private  bool FindPair(TK key, out Pair<TK, TV> found) {
        if (!ContainsKey(key)) {
            found = default(Pair<TK, TV>);
            return false; 
        }
        found = pairs.Find(x => x.key.Equals(key));
        return true;
    }

    public bool ContainsKey(TK key) {
        if (key == null) { throw new ArgumentNullException(); }
        return pairs.Exists(x => x.key.Equals(key));
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

        for (int idx = arrayIndex; idx < array.Length; idx++) {
            Pair<TK, TV> pair = pairs[idx];
            array[idx] = new KeyValuePair<TK, TV>(pair.key, pair.value);
        }
    }


    public IEnumerator<KeyValuePair<TK, TV>> GetEnumeratorObject() {
        return new Enumerator(this);
    }

    public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() {
        return GetEnumeratorObject();
    }

    public bool Remove(TK key) {
        if (key == null) { throw new ArgumentNullException(); }
        if (IsReadOnly) { throw new NotSupportedException(); }
        if (!ContainsKey(key)) { return false; }
        int idx = FindIndex(key);
        pairs.RemoveAt(idx);
        version++;
        return true;
    }

    public bool Remove(KeyValuePair<TK, TV> item) {
        if (item.Key == null) { throw new ArgumentNullException(); }
        if (IsReadOnly) { throw new NotSupportedException(); }
        if (!ContainsKey(item.Key)) { return false; }
        for (int idx = 0; idx < pairs.Count; idx++) {
            Pair<TK, TV> pair = pairs[idx];
            if (item.Key.Equals(pair.key) && item.Value.Equals(pair.value)) {
                pairs.RemoveAt(idx);
                version++;
                return true;
            }
        }
        return false;
    }

    public bool TryGetValue(TK key, out TV value) {
        if (key == null) { throw new ArgumentNullException(); }
        foreach (Pair<TK, TV> pair in pairs) {
            if (pair.key.Equals(key)) {
                value = pair.value;
                return true;
            }
        }
        value = default(TV);
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumeratorObject();
    }


    public struct Enumerator : IEnumerator<KeyValuePair<TK, TV>>
    {
        private readonly EditableDictionary<TK, TV> dictonary;
        private int version;
        private int idx;
        private KeyValuePair<TK, TV> current;

        public KeyValuePair<TK, TV> Current {
            get { return current; }
        }

        internal Enumerator(EditableDictionary<TK, TV> dictionary) {
            dictonary = dictionary;
            version = dictionary.version;
            current = default(KeyValuePair<TK, TV>);
            idx = 0;
        }

        public bool MoveNext() {
            if (version != dictonary.version)
                throw new InvalidOperationException(string.Format("Enumerator version {0} != Dictionary version {1}", version, dictonary.version));

            while (idx < dictonary.Count) {
                Pair<TK, TV> pair = dictonary.pairs[idx];
                current = new KeyValuePair<TK, TV>(pair.key, pair.value);
                idx++;
                return true;
            }

            idx = dictonary.Count + 1;
            current = default(KeyValuePair<TK, TV>);
            return false;
        }

        void IEnumerator.Reset() {
            if (version != dictonary.version)
                throw new InvalidOperationException(string.Format("Enumerator version {0} != Dictionary version {1}", version, dictonary.version));

            idx = 0;
            current = default(KeyValuePair<TK, TV>);
        }

        object IEnumerator.Current {
            get { return Current; }
        }

        public void Dispose() {
        }
    }
}

