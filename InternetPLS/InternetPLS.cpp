#include <WinSock2.h>
#include <ws2ipdef.h>
#include <iphlpapi.h>
#include <cstdio>
#include <iostream>

#pragma comment(lib, "iphlpapi.lib")

int main()
{

    //ULONG out_buf_len = 0;
    //const ULONG flags = GAA_FLAG_INCLUDE_PREFIX;
    //const ULONG family = AF_INET;

    //
	
	
    //DWORD wRetVal = GetAdaptersAddresses(family, flags, nullptr, nullptr, &out_buf_len);

    //auto* p_addresses = static_cast<IP_ADAPTER_ADDRESSES*>(malloc(out_buf_len));

    //GetAdaptersAddresses(family, flags, nullptr, p_addresses, &out_buf_len);

    //IP_ADAPTER_ADDRESSES* cur_address = p_addresses;

    //if (cur_address == nullptr) {
    //    printf("Could not fetch adapter info!");
    //    return 0;
    //}

    //
    //while ((cur_address = cur_address->Next) != nullptr) {

    //    std::wstring name = cur_address->Description;
    //    std::wstring dns_suffix = cur_address->DnsSuffix;

    //    const bool is_HTL = dns_suffix == L"htl.grieskirchen.local";
    //	
    //    std::wcout << name << " | " << dns_suffix << (is_HTL?" <<<<<<<":"") << std::endl;
    //    
    //}
    //free(p_addresses);














	
    //ULONG ulOutBufLen = 0;
    //IP_ADAPTER_INFO* pInfo = nullptr;

    //if (GetAdaptersInfo(nullptr, &ulOutBufLen) == ERROR_BUFFER_OVERFLOW) {
    //    pInfo = static_cast<IP_ADAPTER_INFO*>(malloc(ulOutBufLen));
    //}


    //const DWORD dw_ret_val = GetAdaptersInfo(pInfo, &ulOutBufLen);

    //while (pInfo != nullptr && (pInfo = pInfo->Next))
    //{
    //    char* desc = pInfo->Description;
    //    char* adapterName = pInfo->AdapterName;
    //    char* ipAddress = pInfo->IpAddressList.IpAddress.String;


    //	
    //    std::cout << desc << " | " << ipAddress << std::endl;
    //    
    //}
	
    //if (dw_ret_val == NO_ERROR && pInfo != nullptr) {
    //	
    //    //printf("Number of Adapters: %ld\n\n", pInfo->);
    //    //for (i = 0; i < pInfo->NumAdapters; i++) {
    //    //    printf("Adapter Index[%d]: %ld\n", i,
    //    //        pInfo->Adapter[i].Index);
    //    //    printf("Adapter Name[%d]: %ws\n\n", i,
    //    //        pInfo->Adapter[i].Name);
    //    //}
    //    //iReturn = 0;
    //}
    //else if (dw_ret_val == ERROR_NO_DATA) {
    //    printf
    //    ("There are no network adapters with IPv4 enabled on the local system\n");
    //    return 0;
    //}
    //else {
    //    printf("GetInterfaceInfo failed with error: %lu\n", dw_ret_val);
    //    return 1;
    //}

	
    //free(pInfo);
    //return 0;
}