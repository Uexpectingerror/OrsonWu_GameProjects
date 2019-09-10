#ifndef HEAP_H
#define HEAP_H

#include <vector>
#include <functional>
#include <utility>
#include <algorithm>
#include <stdexcept>
#include <unordered_map>
#include <iostream>
template <
         typename T,
         typename KComparator = std::equal_to<T>,
         typename PComparator = std::less<T>,
         typename Hasher = std::hash<T> >
class Heap
{
public:
    /// Constructs an m-ary heap. M should be >= 2
    Heap(int m = 2,
         const PComparator& c = PComparator(),
         const Hasher& hash = Hasher(),
         const KComparator& kcomp = KComparator()  );

    /// Destructor as needed
    ~Heap();

    /// Adds an item
    void push(const T& item);

    /// returns the element at the top of the heap
    ///  max (if max-heap) or min (if min-heap)
    T const & top() const;

    /// Removes the top element
    void pop();

    /// returns true if the heap is empty
    bool empty() const;

    /// decreaseKey - Finds key matching old object &
    /// updates its location in the heap based on the new value
    void decreaseKey(const T& old, const T& newVal);

    void print()
    {
        for (auto i: keyToLocation_)
        {
            std::cout<<i.first<<" :  "<<i.second << std::endl;
        }
        for(unsigned int i =0; i<store_.size(); ++i)
        {
            std::cout<< i <<" : "<<store_[i]<<std::endl;
        }
    }
 private:
    /// Add whatever helper functions and data members you need below
    void hswap(T& a, T&b);
    void heapify(std::vector<T>& data, size_t loc, size_t effsize, PComparator& c);
    void traversalDown(std::vector<T>& data, size_t loc, size_t effsize, PComparator& c);
    void tickleUp (int loc);

 private:
   /// Data members - These should be sufficient
    std::vector< T > store_;
    int m_;
    PComparator c_;
    std::unordered_map<T, size_t, Hasher, KComparator> keyToLocation_;

};


//helper functions
//tickle up helper function 
template <typename T, typename KComparator, typename PComparator, typename Hasher >
void Heap<T,KComparator,PComparator,Hasher>::tickleUp(int loc)
{
  if(loc <=0)
  {
    keyToLocation_.insert(std::make_pair(store_[loc], loc));//return and add the current value when its at the root 
    return;
  }
  int p_pos = (loc-1)/m_;

  if(c_(store_[loc], store_[p_pos])) // if cur val smaller than the parent value pop it up 
  {
    keyToLocation_[store_[p_pos]] = loc; //changed the value of parent value position
    hswap(store_[loc], store_[p_pos]);
    tickleUp(p_pos);
  }
  else if(c_(store_[p_pos],store_[loc])) // if cur val bigger than the parent value return 
  {
    keyToLocation_.insert(std::make_pair(store_[loc], loc));
    return;
  }
  else // if a duplicate was added to the heap
  {
    keyToLocation_.insert(std::make_pair(store_[loc], loc));
    return; //just return since it is the same value
  }
}

template <typename T, typename KComparator, typename PComparator, typename Hasher >
void Heap<T,KComparator,PComparator,Hasher>::hswap(T& a, T&b)
{
  T temp = a;
  a = b;
  b = temp;
}

template <typename T, typename KComparator, typename PComparator, typename Hasher >
void Heap<T,KComparator,PComparator,Hasher>::traversalDown(std::vector<T>& data, size_t loc, size_t effsize, PComparator& c)
{
  //check if cur position is a non leaf or not
  unsigned int left_c_pos = loc*m_ + 1;
  //if not have a left child pass down to the next heapify
  if( left_c_pos > effsize -1)
  {
    return;
  }
  else 
  {
    int theBetterPos = left_c_pos;
    //compare all the child nodes and find the best one 
    int endChild;
    int c_count;
    if(left_c_pos + m_-1 > effsize -1)
    {
      endChild = effsize-1;
      c_count = endChild - left_c_pos +1;
    }
    else
    {
      c_count = m_;
    }
    
    
    for (int i = 1; i< c_count; ++i)
    {
      if(c(data[left_c_pos+i], data[theBetterPos]))
      {
        theBetterPos = left_c_pos + i;
      }
    } 
    // if the curr node is better than the better node 
    if(c(data[theBetterPos],data[loc]))
    {
      //std::cout <<"swap  "<< "Apos: "<< loc << "  Avalue: "<<mData[loc] << "  VS  Bpos: "<< theBetterPos << "  Bvalue: "<<mData[theBetterPos]<<std::endl;
      keyToLocation_[data[loc]] = theBetterPos;
      keyToLocation_[data[theBetterPos]] = loc;
      hswap(data[loc],data[theBetterPos]);
      traversalDown(data, theBetterPos, effsize, c);
    }
    else
    {
      //std::cout << "pos: "<< loc << "value: "<<mData[loc] <<std::endl;
      return;
    }
  }
}



// heapify() helper function
// loc - Location to start the heapify() process
// effsize - Effective size (number of items in the vector that
//           are part of the heap). Useful for performing heap-sort in place.
template <typename T, typename KComparator, typename PComparator, typename Hasher >
void Heap<T,KComparator,PComparator,Hasher>::heapify(std::vector<T>& data, size_t loc, size_t effsize, PComparator& c )
{
  if(!(loc < effsize))
  {
    return;
  }
  //check if cur position is a non leaf or not
  unsigned int left_c_pos = loc*m_ + 1;
  //if not have a left child pass down to the next heapify
  if( left_c_pos > effsize -1)
  {
    if(loc == 0 )
    {
      return;
    }
    heapify (data, loc-1, effsize, c);
  }
  else
  {
    traversalDown (data, loc, effsize, c);
    if(loc == 0)
    {
      return;
    }
    heapify (data, loc-1, effsize, c);
  }
}


// Complete!
template <typename T, typename KComparator, typename PComparator, typename Hasher >
Heap<T,KComparator,PComparator,Hasher>::Heap(
    int m,
    const PComparator& c,
    const Hasher& hash,
    const KComparator& kcomp ) :

    store_(),
    m_(m),
    c_(c),
    keyToLocation_(100, hash, kcomp)

{

}

// Complete!
template <typename T, typename KComparator, typename PComparator, typename Hasher >
Heap<T,KComparator,PComparator,Hasher>::~Heap()
{

}

// Incomplete!
template <typename T, typename KComparator, typename PComparator, typename Hasher >
void Heap<T,KComparator,PComparator,Hasher>::push( const T& item)
{
    typename std::unordered_map<T, size_t, Hasher, KComparator> :: iterator i = keyToLocation_.find(item);
    if(i!=keyToLocation_.end())
    {
        return;
    }
    store_.push_back(item);
    tickleUp(store_.size()-1);
}

// Incomplete!
template <typename T, typename KComparator, typename PComparator, typename Hasher >
void Heap<T,KComparator,PComparator,Hasher>::decreaseKey(const T& old, const T& newVal)
{
    typename std::unordered_map<T, size_t, Hasher, KComparator> :: iterator i = keyToLocation_.find(old);
    
    if(i!=keyToLocation_.end())
    {
        if(c_(newVal, old))
        {
            int pos = i->second;
            keyToLocation_.erase(old);
            keyToLocation_.insert(std::make_pair(newVal,pos));
            store_[pos] = newVal;
            heapify(store_, pos, store_.size(), c_);
        }
        else 
        {
            return;
        }
    }
    else
    {
        throw std::logic_error("the old key doesn't exist");
    }

}

// Complete!
template <typename T, typename KComparator, typename PComparator, typename Hasher >
T const & Heap<T,KComparator,PComparator,Hasher>::top() const
{
    // Here we use exceptions to handle the case of trying
    // to access the top element of an empty heap
    if(empty()) {
        throw std::logic_error("can't top an empty heap");
    }
    // If we get here we know the heap has at least 1 item
    // Add code to return the top element
    return store_[0];
}

// Incomplete!
template <typename T, typename KComparator, typename PComparator, typename Hasher >
void Heap<T,KComparator,PComparator,Hasher>::pop()
{
    if(empty()) {
        throw std::logic_error("can't pop an empty heap");
    }
    keyToLocation_[store_[store_.size()-1]] = 0;//set the pos of the last value to the root and wait for heapify
    hswap(store_[0], store_[store_.size()-1]);
    keyToLocation_.erase(store_[store_.size()-1]); // clean the value from key to location
    store_.pop_back();
    heapify(store_, 0, store_.size(), c_);
}

/// Complete!
template <typename T, typename KComparator, typename PComparator, typename Hasher >
bool Heap<T,KComparator,PComparator,Hasher>::empty() const
{
    return store_.empty();
}


#endif

