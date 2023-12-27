// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)
//
// Implementation by Saoud Aldowaish

using System;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {

        private Dictionary<string, HashSet<string>> dependentsDic;
        private Dictionary<string, HashSet<string>> dependeesDic;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependentsDic = new Dictionary<string, HashSet<string>>();
            dependeesDic = new Dictionary<string, HashSet<string>>();

        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                int size = 0;
                foreach (KeyValuePair<string, HashSet<string>> pairs in dependentsDic)
                {
                    foreach (string dependee in pairs.Value)
                        size++;
                }
                return size;
            }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (HasDependees(s))
                    return dependeesDic[s].Count();
                else return 0;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            return dependentsDic.ContainsKey(s);
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            return dependeesDic.ContainsKey(s);
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dependentsDic.ContainsKey(s))
                return new HashSet<string>(dependentsDic[s]);
            else return new HashSet<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependeesDic.ContainsKey(s))
                return new HashSet<string>(dependeesDic[s]);
            else return new HashSet<string>();
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {

            // making the needed HashSets
            HashSet<string> dependent = new HashSet<string>();
            HashSet<string> dependee = new HashSet<string>();
            dependent.Add(t);
            dependee.Add(s);

            if (HasDependents(s)) // check if the string s has any dependants
                dependentsDic[s].Add(t);
            else // if not we add a new one to the depndents dictionary
                dependentsDic.Add(s, dependent);

            if (HasDependees(t)) // check if the string t has any dependees
                dependeesDic[t].Add(s);
            else // if not we add a new one to the depndees dictionary
                dependeesDic.Add(t, dependee);
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            // we can't remove a dependency if it doesn't exist
            if (!HasDependents(s) || !HasDependees(t))
                return;

            // removing the dependancy 
            dependentsDic[s].Remove(t);
            dependeesDic[t].Remove(s);

            if (dependentsDic[s].Count == 0) // if s has no more dependents we remove it from the depndents dictionary
                dependentsDic.Remove(s);

            if (dependeesDic[t].Count == 0) // if t has no more depndees we remove it from the depndees dictionary
                dependeesDic.Remove(t);
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (HasDependents(s))
            {
                foreach (string t in GetDependents(s)) 
                    RemoveDependency(s, t);
            }

            foreach (string t in newDependents)
                AddDependency(s, t);
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (HasDependees(s))
            {
                foreach (string t in GetDependees(s))
                    RemoveDependency(t, s);
            }

            foreach (string t in newDependees)
                AddDependency(t, s);
        }

    }

}
