using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

[DefaultMember("Item")]
public class EnumDictionary<TKey, TEnum, TValue> : ICollection<EnumDictionaryTriplet<TKey, TEnum, TValue>> {
  public KeyCollection _keys;
  public ValueCollection _values;
  public EnumCollection _enums;

  public EnumDictionary() {
    _keys = new();
    _values = new();
    _enums = new();
  }

  int Count { get; }
  bool IsReadOnly { get; }


  public bool IsSynchronized => throw new NotImplementedException();

  public object SyncRoot => throw new NotImplementedException();

  int ICollection<EnumDictionaryTriplet<TKey, TEnum, TValue>>.Count => Count;

  bool ICollection<EnumDictionaryTriplet<TKey, TEnum, TValue>>.IsReadOnly => IsReadOnly;

  public IEnumerator GetEnumerator() {
    throw new NotImplementedException();
  }

  void ICollection<EnumDictionaryTriplet<TKey, TEnum, TValue>>.Add(EnumDictionaryTriplet<TKey, TEnum, TValue> item) {
    _keys.Add(item._key);
    _values.Add(item._value);
    _enums.Add(item._enum);
  }

  void ICollection<EnumDictionaryTriplet<TKey, TEnum, TValue>>.Clear() {
    _keys.Clear();
    _values.Clear();
    _enums.Clear();
  }

  public void Add(TKey key, TEnum enumValue, TValue value) {
    Insert(key, enumValue, value, Count);
  }

  private void Insert(TKey key, TEnum enumValue, TValue value, int index) {
  //  if()
  }

  bool ICollection<EnumDictionaryTriplet<TKey, TEnum, TValue>>.Contains(EnumDictionaryTriplet<TKey, TEnum, TValue> item) {
    throw new NotImplementedException();
  }

  public bool ContainsKey(TKey key) {
    return FindEntry(key) >= 0;
  }

  private int FindEntry(TKey key) {
    if (key == null) {
      throw new ArgumentNullException("The key passed was null");
    }
    if (Count != 0) {

    }
    return -1;
  }

  void ICollection<EnumDictionaryTriplet<TKey, TEnum, TValue>>.CopyTo(EnumDictionaryTriplet<TKey, TEnum, TValue>[] array, int arrayIndex) {
    throw new NotImplementedException();
  }

  bool ICollection<EnumDictionaryTriplet<TKey, TEnum, TValue>>.Remove(EnumDictionaryTriplet<TKey, TEnum, TValue> item) {
    throw new NotImplementedException();
  }

  IEnumerator<EnumDictionaryTriplet<TKey, TEnum, TValue>> IEnumerable<EnumDictionaryTriplet<TKey, TEnum, TValue>>.GetEnumerator() {
    throw new NotImplementedException();
  }


  // public sealed class KeyCollection : ICollection<TKey> {
  // [DebuggerTypeProxy(typeof(Mscorlib_DictionaryKeyCollectionDebugView<,>))]
  [DebuggerDisplay("Count = {Count}")]
  [Serializable]
  public sealed class KeyCollection : ICollection<TKey>, ICollection, IReadOnlyCollection<TKey> {

    private EnumDictionary<TKey, TEnum, TValue> dictionary;

    public int Count { get { return dictionary.Count; } }

    bool ICollection<TKey>.IsReadOnly { get { return true; } }

    bool ICollection.IsSynchronized => throw new NotImplementedException();

    object ICollection.SyncRoot => throw new NotImplementedException();

    public KeyCollection(EnumDictionary<TKey, TEnum, TValue> dictionary) {
      if (dictionary == null) {
        throw new ArgumentNullException("Dictionary was null");
      }
      this.dictionary = dictionary;
    }

    public KeyCollection() {
    }

    void ICollection<TKey>.Add(TKey item) { throw new NotSupportedException(); }

    void ICollection<TKey>.Clear() {
      // this.Remove();
    }

    public bool Contains(TKey item) {
      return dictionary.ContainsKey(item);
    }

    public void CopyTo(TKey[] array, int arrayIndex) {
      throw new NotImplementedException();
    }

    public bool Remove(TKey item) {
      throw new NotImplementedException();
    }

    void ICollection.CopyTo(Array array, int index) {
      throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      throw new NotImplementedException();
    }

    IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() {
      throw new NotImplementedException();
    }

    internal void Add(TKey key) {
      throw new NotImplementedException();
    }

    internal void Clear() {
      throw new NotImplementedException();
    }
  }

  public sealed class ValueCollection : ICollection<TValue> {
    private readonly EnumDictionary<TKey, TEnum, TValue> dictionary;

    public int Count { get { return dictionary.Count; } }

    public bool IsReadOnly => throw new NotImplementedException();

    public void Add(TValue item) {
      throw new NotImplementedException();
    }

    public void Clear() {
      throw new NotImplementedException();
    }

    public bool Contains(TValue item) {
      throw new NotImplementedException();
    }

    public void CopyTo(TValue[] array, int arrayIndex) {
      throw new NotImplementedException();
    }

    public IEnumerator<TValue> GetEnumerator() {
      throw new NotImplementedException();
    }

    public bool Remove(TValue item) {
      throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }

  public sealed class EnumCollection : ICollection<TEnum> {
    private readonly EnumDictionary<TKey, TEnum, TValue> dictionary;

    public int Count { get { return dictionary.Count; } }

    public bool IsReadOnly => throw new NotImplementedException();

    public void Add(TEnum item) {
      throw new NotImplementedException();
    }

    public void Clear() {
      throw new NotImplementedException();
    }

    public bool Contains(TEnum item) {
      throw new NotImplementedException();
    }

    public void CopyTo(TEnum[] array, int arrayIndex) {
      throw new NotImplementedException();
    }

    public IEnumerator<TEnum> GetEnumerator() {
      throw new NotImplementedException();
    }

    public bool Remove(TEnum item) {
      throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }
}
public struct EnumDictionaryTriplet<triplet_TKey, triplet_TEnum, triplet_TValue> : ICollection {
  public triplet_TKey _key { get; }
  public triplet_TEnum _enum { get; }
  public triplet_TValue _value { get; }

  public int Count => throw new NotImplementedException();

  public bool IsSynchronized => throw new NotImplementedException();

  public object SyncRoot => throw new NotImplementedException();

  public void Add(EnumDictionaryTriplet<triplet_TKey, triplet_TEnum, triplet_TValue> triplet) {
  }

  public void CopyTo(Array array, int index) {
    throw new NotImplementedException();
  }

  public IEnumerator GetEnumerator() {
    throw new NotImplementedException();
  }
}