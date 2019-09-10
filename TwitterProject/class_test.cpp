#include <iostream>
#include <sstream>
#include <string>
#include "tweet.h"
#include "user.h"
#include "datetime.h"
#include "twiteng.h"
#include "util.h"
#include "hsort.h"
#include <functional>
#include "heap.h"
#include "hash.h"


using namespace std;

 int main(int argc, char *argv[])
 {
 	/**
 	*TEST User class
 	*/
 // 	User u_dcons;
 // 	User u_pcons1("Orson");
 // 	User u_pcons2("Wu");
	// User* u_pointer = new User(u_pcons1);
	// User u_assOp = u_pcons2;
	
	// cout<<"u_dcons: "<<u_dcons.name()<<endl;
	// cout<<"u_pcons1: "<<u_pcons1.name()<<endl;
	// cout<<"u_pcons2: "<<u_pcons2.name()<<endl;
	// cout<<"u_pointer: "<<u_pointer->name()<<endl;
	// cout<<"u_assOp: "<<u_assOp.name()<<endl;
	/**
 	*TEST Datetime class
 	*/

	// DateTime zeroTS;
	// DateTime today(13, 39, 20, 2019, 6, 8);
	// DateTime yesterday(13, 39, 20, 2019, 6, 7);

	// DateTime aDay;
	// cout<<"Today: "<<today<<endl;
	// cout<<"Today > yesterday? "<< (yesterday<today)<<endl;
	// cout<<"input a date stamp pls"<< endl;
	// cin>>aDay;
	// cout<<"The date u inputed: "<< endl;
	// cout<< aDay<< endl;

	/**
 	*TEST Tweet class
 	*/
	// Tweet t_dcons;
	// string tweet = "cs104 is hard and #dsfs annoying!!!#sdf #kjhj #sdfsa ";
	// Tweet t_pcons(u_pointer, today, tweet);
	// Tweet t_ccons(t_pcons);
	// cout<<t_pcons<<endl;
	// cout<<t_ccons<<endl;
	// set<string> hashtags = t_pcons.hashTags();

	// for (set<string>::iterator i = hashtags.begin(); i!= hashtags.end(); ++i)
	// {
	// 	cout<< *i <<endl;
	// }

	// DateTime d1(13, 39, 20, 2019, 6, 7);
	// DateTime d2(13, 39, 20, 2017, 6, 7);
	// DateTime d3(13, 29, 20, 2019, 6, 7);
	// DateTime d4(1, 39, 20, 2018, 6, 7);
	// Tweet* t1 = new Tweet (u_pointer, d1, "this is day1");
	// Tweet* t2 = new Tweet(u_pointer, d2, "this is day2");
	// Tweet* t3 = new Tweet(u_pointer, d3, "this is day3");
	// Tweet* t4 = new Tweet(u_pointer, d4, "this is day4");

	// u_pointer->addTweet(t1);
	// u_pointer->addTweet(t2);
	// u_pointer->addTweet(t3);
	// u_pointer->addTweet(t4);

	// vector<Tweet*> sortedTweets = u_pointer->getFeed();
	// for (auto i: sortedTweets)
	// {
	// 	cout<<*i<<endl;
	// }

	// TwitEng mEng;
	// mEng.parse(argv[1]);
	// mEng.dumpFeeds();
	// vector<string> strings;
	// strings.push_back("football");
	// strings.push_back("punny");
	// strings.push_back("selma");

	// vector <Tweet*> searchResult = mEng.search(strings, 0);
	// displayHits(searchResult);

	std::vector<int> intTestV =
	{3,0,87,30,564,34,45,2,23,43,67,56,84,45,90,456,6564,1,4,6,46,75};
	// hsort(intTestV, greater<int>());
	// for (auto i : intTestV)
	// {
	// 	cout<< i << endl;
	// }

	cout<<"Below is heap testing stuff haahha!"<<endl;
	Heap<int> test1(4);

	for (auto i : intTestV)
	{
		test1.push(i);
	}
	test1.decreaseKey(6564, -300);
	test1.decreaseKey(43, -300);
	test1.print();
	//test1.printData();
	while (!test1.empty())
	{
		cout<<test1.top()<<endl;
		test1.pop();
	}

	unsigned int x = calculateHash("ifuckyou");
	cout<<x<<endl;

 }
