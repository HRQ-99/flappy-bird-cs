using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[DefaultMember("Item")]
public class EnumDictionary<TKey, TEnum, TValue> : ICollection<> {
  public List<TKey> _keys;
  public List<TValue> _values;
  public List<TEnum> _enums;

  public EnumDictionary() {
    _keys = new();
    _values = new();
    _enums = new();
  }

  int Count { get; }
  bool IsReadOnly { get; }

  int ICollection.Count => Count;

  public bool IsSynchronized => throw new NotImplementedException();

  public object SyncRoot => throw new NotImplementedException();

  public void CopyTo(Array array, int index) {
    throw new NotImplementedException();
  }

  public IEnumerator GetEnumerator() {
    throw new NotImplementedException();
  }












  public struct EnumDictionaryTriplet<triplet_TKey, triplet_TEnum, triplet_TValue> {
    TKey _key { get; }
    TEnum _enum { get; }
    TValue _value { get; }
  }
}