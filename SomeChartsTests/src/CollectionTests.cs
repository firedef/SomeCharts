using System;
using System.Collections.Generic;
using NUnit.Framework;
using SomeChartsUi.utils.collections;

namespace SomeChartsTests; 

[TestFixture]
public class CollectionTests {
	[Test]
	public void TestNativeListInsertion() {
		using NativeList<int> l = new();
		l.Insert(0, 5);
		Assert.AreEqual(l[0], 5);
		
		l.Insert(0, 3);
		Assert.AreEqual(l[0], 3);
		Assert.AreEqual(l[1], 5);
		Assert.AreEqual(l.count, 2);
		
		l.Insert(2, 4);
		Assert.AreEqual(l[0], 3);
		Assert.AreEqual(l[1], 5);
		Assert.AreEqual(l[2], 4);
		Assert.AreEqual(l.count, 3);
		
		l.RemoveAt(1);
		Assert.AreEqual(l[0], 3);
		Assert.AreEqual(l[1], 4);
		Assert.AreEqual(l.count, 2);
		
		l.RemoveAt(0);
		Assert.AreEqual(l[0], 4);
		Assert.AreEqual(l.count, 1);
	}
	
	[Test]
	public void TestNativeListAlloc() {
		using NativeList<int> l = new(0);
		
		Assert.AreEqual(l.capacity, 0);
		
		l.Add(4);
		Assert.AreEqual(l[0], 4);
		Assert.AreEqual(l.count, 1);
		Assert.AreEqual(l.capacity, 1);
		
		l.Add(7);
		Assert.AreEqual(l[0], 4);
		Assert.AreEqual(l[1], 7);
		Assert.AreEqual(l.count, 2);
		Assert.AreEqual(l.capacity, 2);
		
		l.Add(9);
		Assert.AreEqual(l[0], 4);
		Assert.AreEqual(l[1], 7);
		Assert.AreEqual(l[2], 9);
		Assert.AreEqual(l.count, 3);
		Assert.AreEqual(l.capacity, 4);
	}
	
	[Test]
	public void TestNativeListContains() {
		using NativeList<int> l = new();

		for (int i = 0; i < 32; i++) l.Add(i);
		
		Assert.AreEqual(l.IndexOf(7), 7);
		Assert.AreEqual(l.IndexOf(17), 17);
		Assert.AreEqual(l.IndexOf(0), 0);
		Assert.AreEqual(l.IndexOf(33), -1);
		Assert.AreEqual(l.IndexOf(-1), -1);
		Assert.IsTrue(l.Contains(9));
		Assert.IsTrue(l.Contains(0));
		Assert.IsFalse(l.Contains(37));
	}
	
	[Test]
	public void TestNativeListEnumeration() {
		using NativeList<int> l = new() {0, 1, 2, 3};

		for (int i = 4; i < 32; i++) l.Add(i);

		int iter = 0;
		foreach (int i in l) {
			Assert.AreEqual(iter, i);
			iter++;
		}
		
		Assert.AreEqual(iter, l.count);
		Assert.AreEqual(iter, 32);
	}
	
	[Test]
	public void TestHashedList() {
		using HashedList<int> l = new();
		
		for (int i = 0; i < 64; i++) l.Add(i);
		l.ResetChanges();

		Assert.IsFalse(l.GetCountChange());
		List<Range> ch = l.GetChanges();
		Assert.AreEqual(ch.Count, 0);

		l[1] = 1;
		Assert.IsFalse(l.GetCountChange());
		ch = l.GetChanges();
		Assert.AreEqual(ch.Count, 0);
		
		l[1] = 2;
		Assert.IsFalse(l.GetCountChange());
		ch = l.GetChanges();
		Assert.AreEqual(ch.Count, 1);
		Assert.AreEqual(ch[0], ..HashedList<int>.blockSize);
		
		Assert.IsFalse(l.GetCountChange());
		ch = l.GetChanges();
		Assert.AreEqual(ch.Count, 0);

		for (int i = 0; i < 64; i++) l[i] = 64 - i;
		Assert.IsFalse(l.GetCountChange());
		ch = l.GetChanges();
		Assert.AreEqual(ch.Count, 1);
		Assert.AreEqual(ch[0], ..64);
		
		l.Add(1);
		Assert.IsTrue(l.GetCountChange());
		ch = l.GetChanges();
		Assert.AreEqual(ch.Count, 1);
		Assert.AreEqual(ch[0], ..65);
	}
	
	[Test]
	public void TestHashedListRandomValues() {
		using HashedList<int> l = new();
		const int c = 512;
		Random rnd = new();
		
		for (int i = 0; i < c; i++) l.Add(i);
		l.ResetChanges();

		for (int i = 0; i < 100; i++) {
			int ind = rnd.Next(c);
			l[ind] = rnd.Next(c);
		}
		
		Assert.IsFalse(l.GetCountChange());
		
		List<Range> ch = l.GetChanges();
		foreach (Range r in ch) {
			Console.WriteLine(r);
		}
	}

}