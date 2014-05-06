
#include "stdafx.h"

#include "PluginWrapper.h"


extern "C" __declspec(dllexport) 
bool __cdecl isUnicode()
{
	return NppDB::PluginWrapper::Instance()->isUnicode();
}

extern "C" __declspec(dllexport)
void __cdecl setInfo(NppData1 notepadPlusData)
{
	NppDB::PluginWrapper::Instance()->setInfo(notepadPlusData);
}
	
extern "C" __declspec(dllexport) 
void * __cdecl getFuncsArray(int *nbF)
{
	return NppDB::PluginWrapper::Instance()->getFuncsArray(nbF);
}

extern "C" __declspec(dllexport) 
LRESULT __cdecl messageProc(UINT Message, WPARAM wParam, LPARAM lParam)
{
	return NppDB::PluginWrapper::Instance()->messageProc(Message,wParam,lParam);
}

extern "C" __declspec(dllexport)
const char * __cdecl getName()
{
	return NppDB::PluginWrapper::Instance()->getName();
}
extern "C" __declspec(dllexport)
void __cdecl beNotified(void * notifyCode)
{
	NppDB::PluginWrapper::Instance()->beNotified(notifyCode);
}
	