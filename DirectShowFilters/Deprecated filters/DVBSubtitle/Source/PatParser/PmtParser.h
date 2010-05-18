/* 
 *	Copyright (C) 2006-2008 Team MediaPortal
 *	http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */
#pragma once
#include "sectiondecoder.h"
#include "PidTable.h"
#include <map>
using namespace std;

#define SERVICE_TYPE_VIDEO_MPEG1		0x1
#define SERVICE_TYPE_VIDEO_MPEG2		0x2
#define SERVICE_TYPE_VIDEO_MPEG4		0x10
#define SERVICE_TYPE_VIDEO_H264		  0x1b
#define SERVICE_TYPE_AUDIO_MPEG1		0x3
#define SERVICE_TYPE_AUDIO_MPEG2		0x4
#define SERVICE_TYPE_AUDIO_AC3			0x81 //fake

#define SERVICE_TYPE_DVB_SUBTITLES1	0x5
#define SERVICE_TYPE_DVB_SUBTITLES2	0x6

#define DESCRIPTOR_DVB_AC3				  0x6a
#define DESCRIPTOR_DVB_TELETEXT     0x56
#define DESCRIPTOR_DVB_SUBTITLING		0x59
#define DESCRIPTOR_MPEG_ISO639_Lang 0x0a

class IPmtCallBack
{
public:
	virtual void OnPmtReceived(int pmtPid)=0;
};

class CPmtParser: public  CSectionDecoder
{
public:
  CPmtParser(void);
  virtual ~CPmtParser(void);
  CPidTable& GetPidInfo();
	void			 OnNewSection(CSection& sections);
  bool			 Ready();
	void       SetPmtCallBack(IPmtCallBack* callback);
private:
  int				m_pmtPid;
  CPidTable m_pidInfo;
	bool			_isFound;
	IPmtCallBack* m_pmtCallback;
};
