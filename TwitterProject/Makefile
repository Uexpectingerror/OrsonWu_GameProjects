CC = g++
CPPFLAGS = -g -Wall -std=c++11
CPPFLAGS_O = -g -Wall -c -std=c++11
BIN_DIR = bin
TEST_DIR = tests
GTEST_LL = -I /usr/include/gtest/ -l gtest -l gtest_main -pthread


All: twitter class_test

twitter: twitter.cpp user.o tweet.o datetime.o util.o twiteng.o handler.o cmdhandler.o hsort.h hash.o
	$(CC) $(CPPFLAGS) $^ -o $@

class_test: class_test.cpp user.o tweet.o datetime.o util.o twiteng.o hsort.h heap.h hash.o
	$(CC) $(CPPFLAGS) $^ -o $@

tweet.o: tweet.h tweet.cpp user.h
	$(CC) $(CPPFLAGS_O) tweet.cpp -o tweet.o 

user.o: user.h user.cpp tweet.h hsort.h
	$(CC) $(CPPFLAGS_O) user.cpp  -o user.o

datetime.o: datetime.h datetime.cpp
	$(CC) $(CPPFLAGS_O) datetime.cpp -o datetime.o

util.o: util.h util.cpp
	$(CC) $(CPPFLAGS_O) util.cpp -o $@

twiteng.o: twiteng.h twiteng.cpp user.h tweet.h datetime.h hsort.h heap.h tagdata.h
	$(CC) $(CPPFLAGS_O) twiteng.cpp -o $@

handler.o: handler.h handler.cpp
	$(CC) $(CPPFLAGS_O) handler.cpp -o $@

cmdhandler.o: cmdhandler.h cmdhandler.cpp 
	$(CC) $(CPPFLAGS_O) cmdhandler.cpp -o $@

hash.o: hash.h hash.cpp 
	$(CC) $(CPPFLAGS_O) hash.cpp -o $@
