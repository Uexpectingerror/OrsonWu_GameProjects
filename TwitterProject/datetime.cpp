#include "datetime.h"
#include <sstream>
#include <string>
#include <iomanip>
#include <istream>  
#include "util.h"
#include <ctime>
using namespace std;
/**
 * Default constructor for current system time/date
 */
DateTime::DateTime()
:hour(0), min(0), sec(0), year(0), month(0), day(0)
{
  setTimeLocal(*this);
}

/**
 * Alternative constructor 
 */
DateTime::DateTime(int hh, int mm, int ss, int year, int month, int day)
:hour(hh), min(mm), sec(ss), year(year), month(month), day(day)
{

}

/**
 * Return true if the timestamp is less-than other's
 *
 * @return result of less-than comparison of timestamp
 */
bool DateTime::operator<(const DateTime& other) const
{
  string myTs = to_string(year) + to_string(month) +to_string(day) +to_string(hour) +to_string(min) +to_string(sec);
  string oTs = to_string(other.year) + to_string(other.month) +to_string(other.day) +to_string(other.hour) +to_string(other.min) 
  +to_string(other.sec);
  // cout <<"myTs: "<< myTs<<endl;
  // cout <<"oTs: "<< oTs<<endl;

  if(myTs < oTs)
  {
    return true;
  }
  else 
  {
    return false;
  }
}




/**
 * Outputs the timestamp to the given ostream in format:
 *   YYYY-MM-DD HH::MM::SS
 *
 * @return the ostream passed in as an argument
 */
std::ostream& operator<<(std::ostream& os, const DateTime& other)
{
  os << other.year <<"-"<<std::setfill ('0') << std::setw (2) << other.month << "-" 
  <<std::setfill ('0') << std::setw (2) << other.day 
  <<" " << std::setfill ('0') << std::setw (2) <<other.hour<<":" 
  <<std::setfill ('0') << std::setw (2) <<other.min << ":"
  <<std::setfill ('0') << std::setw (2) <<other.sec;
  return os;
}

//local time helper func
void setTimeLocal (DateTime& dt)
{
  //calculate current time in case tbe format is wrong
  time_t rawtime;
  struct tm * timeinfo;
  time (&rawtime);
  timeinfo = localtime(&rawtime);
  dt.year = timeinfo->tm_year + 1900;
  dt.month = timeinfo->tm_mon +1;
  dt.day = timeinfo->tm_mday;
  dt.hour = timeinfo->tm_hour;
  dt.min = timeinfo->tm_min;
  dt.sec = timeinfo->tm_sec;
}

/**
 * Inputs the timestamp to the given istream expecting the format:
 *   YYYY-MM-DD HH:MM:SS
 *  Returns if an error in the format with the DateTime set to the
 *   current system time/date
 *   
 *
 * @return the istream passed in as an argument
 */
std::istream& operator>>(std::istream& is, DateTime& dt)
{
  //exam if the format if correct
  //trim the is and put it into sstream
  string ls;
  getline(is, ls);
  ls = trim(ls);
  stringstream ss(ls);
  
  //parse it into two parts
  string symd;
  string shms;
  ss >> symd;
  ss >> shms; 

  stringstream ssymd(symd);
  stringstream sshms(shms);

  string y;
  string mt;
  string d;
  string h;
  string mi;
  string s;

  if(symd.size()!=10 || shms.size()!=8)
  {
    setTimeLocal(dt);
    return is;
  }
  string temp;
  int i =0;
  //input all the strings to y m d
  while (getline(ssymd, temp, '-'))
  {
    i++;
    if(i==1)
    {
      if(temp.size()==4 && temp.find_first_not_of("0123456789") == std::string::npos)
      {
        y = temp;
      }
      else
      {
        setTimeLocal(dt);
        return is;
      }
    }
    else if (i==2)
    {
      if(temp.size()==2 && temp.find_first_not_of("0123456789") == std::string::npos)
      {
        mt = temp;
      }
      else
      {
        setTimeLocal(dt);
        return is;
      }
    }
    else if (i==3)
    {
      if(temp.size()==2 && temp.find_first_not_of("0123456789") == std::string::npos)
      {
        d = temp;
      }
      else
      {
        setTimeLocal(dt);
        return is;
      }
    }
  }
  temp = "";
  i = 0;
  //input all the strings to r m s
  while (getline(sshms, temp, ':'))
  {
    i++;
    if(i==1)
    {
      if(temp.size()==2 && temp.find_first_not_of("0123456789") == std::string::npos)
      {
        h = temp;
      }
      else
      {
        setTimeLocal(dt);
        return is;
      }
    }
    else if (i==2)
    {
      if(temp.size()==2 && temp.find_first_not_of("0123456789") == std::string::npos)
      {
        mi = temp;
      }
      else
      {
        setTimeLocal(dt);
        return is;
      }
    }
    else if (i==3)
    {
      if(temp.size()==2 && temp.find_first_not_of("0123456789") == std::string::npos)
      {
        s = temp;
      }
      else
      {
        setTimeLocal(dt);
        return is;
      }
    }
  }

  //turn all the strings to int and set them in m variables 
  dt.year = stoi(y);
  dt.month = stoi(mt);
  dt.day = stoi(d);

  dt.hour = stoi(h);
  dt.min = stoi(mi);
  dt.sec = stoi(s);
  return is;
}
