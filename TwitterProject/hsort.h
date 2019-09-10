#include <iostream>
#include <vector>
  #include <string>


template <typename T>
void swap(T& a, T&b)
{
	T temp = a;
	a = b;
	b = temp;
}

template <typename T, typename Comparator >
void traversalDown(std::vector<T>& data, size_t loc, size_t effsize, Comparator& c)
{
	//check if cur position is a non leaf or not
	unsigned int left_c_pos = loc*2 + 1;
	//if not have a left child pass down to the next heapify
	if( left_c_pos > effsize -1)
	{
		return;
	}
	else 
	{
		//if only have the left child and the child is better than the parent then swap them
		if (left_c_pos+1 > effsize -1 )
		{
			if(c(data[loc],data[left_c_pos]))
			{
				swap (data[loc],data[left_c_pos]);
				traversalDown(data, left_c_pos, effsize, c);
			}
			else
			{
				return;
			}
			
		}
		else 
		{
			int theBetterPos;
			if (c(data[left_c_pos],data[left_c_pos+1]))
			{
				theBetterPos = left_c_pos+1;
			}
			else
			{
				theBetterPos = left_c_pos;
			}	

			if(c(data[loc],data[theBetterPos]))
			{
				swap (data[loc],data[theBetterPos]);
				traversalDown(data, theBetterPos, effsize, c);
			}
			else
			{
				return;
			}
		}
	}
}



// heapify() helper function
// loc - Location to start the heapify() process
// effsize - Effective size (number of items in the vector that
//           are part of the heap). Useful for performing heap-sort in place.
template <typename T, typename Comparator >
void heapify(std::vector<T>& data, size_t loc, size_t effsize, Comparator& c )
{
	if(!(loc < effsize))
	{
		return;
	}
	//check if cur position is a non leaf or not
	unsigned int left_c_pos = loc*2 + 1;
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




template <typename T, typename Comparator >
void hsort(std::vector<T>& data, Comparator c = Comparator())
{
	heapify(data, (data.size()-1), data.size(), c);
	for(unsigned int i = 0; i<data.size(); ++i)
	{
		T tempBest = data[0];
		data[0] = data[data.size()-i-1];
		data[data.size()-i-1] = tempBest;
		heapify(data, 0, data.size()-i-1,c);
	} 
}
