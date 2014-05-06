// PluginLoader.h

#include <windows.h>
#pragma unmanaged
typedef	struct NppData1 {
		HWND _nppHandle;
		HWND _scintillaMainHandle;
		HWND _scintillaSecondHandle;
	};

#pragma once

using namespace System;
using namespace System::IO;
using namespace System::Reflection;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

namespace NppDB {
	
#pragma managed
	/*
	Dictionary<String^,MethodInfo^>^ _miSet = gcnew Dictionary<String^,MethodInfo^>(6);
	String^ _asmPath = L".\\NppDB\\NppDB.dll";
	String^ _typeName = L"NPPExports";

	void checkExportMethod(String^ methodName)
	{
		 Assembly^ asm = Assembly::LoadFrom(_asmPath);
		 _miSet[methodName] = asm->GetType(_typeName)->GetMethod(methodName);
	}

	void CallMethod(String^ name)
	{

	}
	*/
	
	/*
	extern "C" __declspec(dllexport)  class npwrapper
	{
	private:

	public :
		static  void __cdecl test()
		{
			NppDB::NPPExports::isUnicode();
		}
	};
	*/

	
	
	ref class PluginWrapper
	{
	private:
		Type^ _typ;
		Type^ _typNppData;
		Type^ _typSCNotification;
		Object^ _pluginObj;
		String^ _pluginName;
		String^ _assemblyName;
		static PluginWrapper^ _inst = nullptr;

		void checkType(void);
		void checkTypeNppData(void);
		void checkSCNotification(void);

	public:
		NppDB::PluginWrapper()
		{
			_typ = nullptr;
			_typNppData = nullptr;
			_typSCNotification = nullptr;
			_pluginObj = nullptr;
			_pluginName = "NppDB";
			_assemblyName = L"NppDB.Plugin.dll";
		}
		bool isUnicode(void);
		void  setInfo(NppData1 notepadPlusData);
		void * getFuncsArray(int *nbF);
		bool messageProc(UINT Message, WPARAM wParam, LPARAM lParam);
		const char * getName(void);
		void beNotified(void * notifyCode);
		static PluginWrapper^ Instance(void);

	};
	

}

