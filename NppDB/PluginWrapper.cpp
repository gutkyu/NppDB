#include "stdafx.h"

#include "PluginWrapper.h"

void NppDB::PluginWrapper::checkType()
{
	if(_typ == nullptr)
	{
		String^ path = Path::Combine( Path::GetDirectoryName ((gcnew System::Uri(Assembly::GetExecutingAssembly()->CodeBase))->AbsolutePath), _pluginName ,_assemblyName);
		Assembly^ assem = Assembly::LoadFrom(path);
		_typ =  assem->GetType(L"NppDB.NPPExports");
	}
}

void NppDB::PluginWrapper::checkTypeNppData()
{
	if(_typNppData == nullptr)
	{
		String^ path = Path::Combine( Path::GetDirectoryName ((gcnew System::Uri(Assembly::GetExecutingAssembly()->CodeBase))->AbsolutePath), _pluginName ,_assemblyName);
		Assembly^ assem = Assembly::LoadFrom(path);
		_typNppData =  assem->GetType(L"NppDB.NppData");
	}
}

void NppDB::PluginWrapper::checkSCNotification()
{
	if(_typSCNotification == nullptr)
	{
		String^ path = Path::Combine( Path::GetDirectoryName ((gcnew System::Uri(Assembly::GetExecutingAssembly()->CodeBase))->AbsolutePath), _pluginName ,_assemblyName);
		Assembly^ assem = Assembly::LoadFrom(path);
		_typSCNotification =  assem->GetType(L"NppDB.SCNotification");
	}
}

bool NppDB::PluginWrapper::isUnicode()
{
		checkType();
		return static_cast<bool>( _typ->GetMethod(L"isUnicode")->Invoke(nullptr, nullptr));
}

void  NppDB::PluginWrapper::setInfo(NppData1 notepadPlusData)
{
	checkType();
	checkTypeNppData();
		_typ->GetMethod(L"setInfo")->Invoke(nullptr, gcnew array<Object^> { Marshal::PtrToStructure(IntPtr((void *)(&notepadPlusData)), _typNppData) });
}

void * NppDB::PluginWrapper::getFuncsArray(int *nbF)
{
	checkType();
	array<Object^>^ params = gcnew array<Object^> {*nbF};
	void * ret = static_cast<void *>(( static_cast<IntPtr>(  _typ->GetMethod(L"getFuncsArray")->Invoke(nullptr, params ))).ToPointer());
	*nbF =  static_cast<int>(params[0]);
	return ret;
}
		
bool NppDB::PluginWrapper::messageProc(UINT Message, WPARAM wParam, LPARAM lParam)
{
	return true;
}
		
const char * NppDB::PluginWrapper::getName()
{
	checkType();
	return static_cast<char *>(( static_cast<IntPtr>( _typ->GetMethod(L"getName")->Invoke(nullptr, nullptr ))).ToPointer());
}

void NppDB::PluginWrapper::beNotified(void * notifyCode)
{
	checkType();
	checkSCNotification();
	_typ->GetMethod(L"beNotified")->Invoke(nullptr, gcnew array<Object^> { Marshal::PtrToStructure(IntPtr(notifyCode), _typSCNotification) });
}
