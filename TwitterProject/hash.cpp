#include "hash.h"

using namespace std;

unsigned int calculateHash(string mystring) // mystring must be less than 8 characters
{
	if(mystring.length()>8)
	{
		return 0;
	}
  /* add your code here */
	unsigned long long transStr = 0;
	unsigned int wints [4]; 
	for (unsigned int i =0; i<mystring.length(); ++i)
	{
		transStr = 128*transStr + mystring[i];
	}

	for (int i = 3; i>=0; --i)
	{
		wints[i] = transStr % 65521;
		transStr = transStr / 65521;
	}

	return (45912 * wints[0] + 35511 * wints[1] + 65169 * wints[2] + 4625 * wints[3]) % 65521;
}

