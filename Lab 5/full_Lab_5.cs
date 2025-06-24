//===============================================Lab 5=======================================

//-----------------------------------ArrayList-------------------------------------

//ArrayList arrayList = new ArrayList();
//arrayList.Add(1);
//arrayList.Add(2);
//arrayList.Add("Shreeya");
//arrayList.Add(256);
//arrayList.Add("ghsb");
//Console.WriteLine("after the add data : ");
//foreach (var i in arrayList)
//{
//    Console.WriteLine(i);
//}

//arrayList.Remove(1);
//Console.WriteLine("after remove the data :");
//foreach (var i in arrayList)
//{
//    Console.WriteLine(i);
//}

//arrayList.RemoveAt(0);
//Console.WriteLine("remove at :");
//foreach (var i in arrayList)
//{
//    Console.WriteLine(i);
//}

//arrayList.Clear();
//Console.WriteLine("clear all the data !");
//foreach (var i in arrayList)
//{
//    Console.WriteLine(i);
//}

//----------------------------------------List-----------------------------------------------

//List<String> list = new List<String>();
//list.Add("1");
//list.Add("2");
//list.Add("Shreeya");
//list.Add("256");
//list.Add("ghsb");
//Console.WriteLine("after the add data : ");
//foreach (var i in list)
//{
//    Console.WriteLine(i);
//}

//list.Remove("1");
//Console.WriteLine("after remove the data :");
//foreach (var i in list)
//{
//    Console.WriteLine(i);
//}

//list.RemoveAt(0);
//Console.WriteLine("remove at :");
//foreach (var i in list)
//{
//    Console.WriteLine(i);
//}

//list.Clear();
//Console.WriteLine("clear all the data !");
//foreach (var i in list)
//{
//    Console.WriteLine(i);
//}

//----------------------------------Stack-------------------------------------------------
//Stack stack = new Stack();
//stack.Push(1);
//stack.Push(2);
//stack.Push(3);
//Console.WriteLine("after the add data");
//foreach (var item in stack)
//{
//    Console.WriteLine(item);
//}

//Console.WriteLine("when you have the delete only one data");
//stack.Pop();
//foreach (var item in stack)
//{
//    Console.WriteLine(item);
//}

//Console.WriteLine("contains the data:");
//Console.WriteLine("stack contains elements 2 ? :"+stack.Contains(2));

//foreach (var item in stack)
//{
//    Console.WriteLine(item);
//}

//Console.WriteLine("cleare the data");
//stack.Clear();
//foreach (var item in stack)
//{
//    Console.WriteLine(item);
//}


//--------------------------------Queue-----------------------------------------------

//Queue queue = new Queue();
//queue.Enqueue(1);
//queue.Enqueue(2);
//queue.Enqueue(3);
//queue.Enqueue(4);
//Console.WriteLine("add data in queue : ");
//foreach (var item in queue)
//{
//    Console.WriteLine(item);
//}

//queue.Dequeue();
//Console.WriteLine("remove the data : ");
//foreach (var item in queue)
//{
//    Console.WriteLine(item);
//}

//queue.Peek();
//Console.WriteLine("peek the element using peek method is :");
//foreach (var item in queue)
//{
//    Console.WriteLine(item);
//}

//queue.Clear();
//Console.WriteLine("clear all the data successfull !!");
//foreach (var item in queue)
//{
//    Console.WriteLine(item);
//}

//--------------------------------------Dis-------------------------------------------
//Dictionary<int, string> myDictionary = new Dictionary<int, string>();

//myDictionary.Add(1, "Apple");
//myDictionary.Add(2, "Banana");
//myDictionary.Add(3, "Cherry");

//Console.WriteLine("After Adding Elements:");
//foreach (var kvp in myDictionary)
//{
//    Console.WriteLine("Key: " + kvp.Key + ", Value: " + kvp.Value);
//}

//myDictionary.Remove(2);
//Console.WriteLine("\nAfter Removing Key 2:");
//foreach (var kvp in myDictionary)
//{
//    Console.WriteLine("Key: " + kvp.Key + ", Value: " + kvp.Value);
//}

//Console.WriteLine("\nContains Key 1: " + myDictionary.ContainsKey(1));
//Console.WriteLine("Contains Key 2: " + myDictionary.ContainsKey(2));

//Console.WriteLine("\nContains Value 'Apple': " + myDictionary.ContainsValue("Apple"));
//Console.WriteLine("Contains Value 'Banana': " + myDictionary.ContainsValue("Banana"));

//myDictionary.Clear();
//Console.WriteLine("\nAfter Clearing Dictionary:");

//    Console.WriteLine("Dictionary is empty.");

//-------------------------------Hashtable--------------------------------
//Hashtable hashtable = new Hashtable();
//hashtable.Add("1", "darshan");
//hashtable.Add("2", "university");
//hashtable.Add("3", "rajkot");
//hashtable.Add("4", "hadala");
//ICollection keys = hashtable.Keys;
//foreach (string key in keys)
//{
//    Console.WriteLine(hashtable[key]);
//}

//Console.WriteLine("remove the data");
//hashtable.Remove("1");
//foreach (string key in keys)
//{
//    Console.WriteLine(hashtable[key]);
//}

//Console.WriteLine("Contains key");
//Console.WriteLine("contains key in hashtable :" + hashtable.ContainsKey("2"));


//Console.WriteLine("Contains value");
//Console.WriteLine("contains key in hashtable :" + hashtable.ContainsValue("university"));


//hashtable.Clear();
//foreach (string key in keys)
//{
//    Console.WriteLine(hashtable[key]);
//}
//Console.WriteLine("cleare all data");
