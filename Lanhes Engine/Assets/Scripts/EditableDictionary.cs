using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EditableDictionary<TK, TV> : IDictionary<TK, TV> {

    [SerializeField]
    public List<TK> keys = new List<TK>();

    [SerializeField]
    public List<TV> values = new List<TV>();

    public TV this[TK key] { get => Get(key); set => Set(key, value); }

    private void Set(TK key, TV value) {
        if (key == null) { throw new ArgumentNullException(); }
        if (IsReadOnly) { throw new NotSupportedException(); }
        if (!ContainsKey(key)) { throw new ArgumentException("argument " + key + "not found"); }
        int idx = FindIndex(key);
        values[idx] = value;
        version++;

    }

    private TV Get(TK key) {
        if (key == null) { throw new ArgumentNullException(); }
        if (IsReadOnly) { throw new NotSupportedException(); }
        if (!ContainsKey(key)) { throw new ArgumentException("argument " + key + "not found"); }
        int idx = FindIndex(key);
        return values[idx];
    }

    public ICollection<TK> Keys => new List<TK>(keys);

    public ICollection<TV> Values => new List<TV>(values);

    public int Count => keys.Count;

    //I have no fucking clue why this exists
    public bool IsReadOnly => false;

    private int version = 0;

    public void Add(TK key, TV value) {
        if (!IsReadOnly) {
            if (key == null) { throw new ArgumentNullException(); }
            if (keys.Contains(key)) {
                throw new ArgumentException("Dictionary already contains key " + key);
            } else
            {
                keys.Add(key);
                values.Add(value);
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
            keys.Clear();
            values.Clear();
            version++;
        } else {
            throw new NotSupportedException("Dictionary is read-only");
        }
    }

    public bool Contains(KeyValuePair<TK, TV> item) {
        int index = FindIndex(item.Key);
        return index >= 0 &&
            EqualityComparer<TV>.Default.Equals(values[index], item.Value);
    }

    private int FindIndex(TK key) {
        if (!ContainsKey(key)) { throw new KeyNotFoundException(); }
        return keys.FindIndex(x => x.Equals(key));
    }

    public bool ContainsKey(TK key) {
        if (key == null) { throw new ArgumentNullException(); }
        return keys.Contains(key);

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
            array[idx] = new KeyValuePair<TK, TV>(keys[idx], values[idx]);
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
        keys.RemoveAt(idx);
        values.RemoveAt(idx);
        version++;
        return true;
    }

    public bool Remove(KeyValuePair<TK, TV> item) {
        if (item.Key == null) { throw new ArgumentNullException(); }
        if (IsReadOnly) { throw new NotSupportedException(); }
        if (!ContainsKey(item.Key)) { return false; }
        for (int idx = 0; idx < keys.Count; idx++) {
            if (item.Key.Equals(keys[idx]) && item.Value.Equals(values[idx])) {
                keys.RemoveAt(idx);
                values.RemoveAt(idx);
                version++;
                return true;
            }
        }
        return false;
    }

    public bool TryGetValue(TK key, out TV value) {
        if (key == null) { throw new ArgumentNullException(); }
        int idx = FindIndex(key);
        value = values[idx];
        return true;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumeratorObject();
    }


    public struct Enumerator : IEnumerator<KeyValuePair<TK, TV>> {
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
                current = new KeyValuePair<TK, TV>(dictonary.keys[idx], dictonary.values[idx]);
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

