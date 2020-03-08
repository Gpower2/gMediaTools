// modified by Gpower2, renamed to AcAvisynthWrapper for AnimeClipse
// modified by dimzon, renamed to AvisynthWrapper for futher independent development
// avisynth redirecter dll modified by Inc.
// Original by MobileHackerz http://www.nurs.or.jp/~calcium/

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <io.h>
#include <fcntl.h>
#include <windows.h>
#include "include\avisynth.h"

#define ERRMSG_LEN 1024

#ifndef INT64
#define INT64 long long
#endif

const AVS_Linkage* AVS_linkage = NULL;

//Structure that contains Information about the video file
typedef struct gAvsWrapperVideoFileInfo {
	// Video
	int width;
	int height;
	int rate_numerator;
	int rate_denominator;
	int aspect_numerator;
	int aspect_denominator;
	int has_interlaced_frames;
	int is_top_field_first;
	int number_frames;
	int pixel_type;

	// Audio
	int audio_samples_per_second;
	int sample_type;
	int number_channels;
	int number_audio_frames;
	INT64 number_audio_samples;
} gAvsWrapperVideoFileInfo;

typedef struct gAvsWrapperVideoPlane {
	int width;
	int height;
	int pitch;
	BYTE* data;
} gAvsWrapperVideoPlane;

typedef struct FRAME {
	int* data;
} FRAME;

typedef struct gAvsWrapperVideoFrame {
	gAvsWrapperVideoPlane plane[3];
} gAvsWrapperVideoFrame;

typedef struct gAvsWrapperAudioPosition {
	INT64 start;
	INT64 count;
} gAvsWrapperAudioPosition;

typedef struct tagSafeStruct
{
	char error_msg[ERRMSG_LEN];
	IScriptEnvironment* environment;
	AVSValue* result;
	PClip clp;
	HMODULE dll;	
}SafeStruct;

extern "C" {
	__declspec(dllexport) int __stdcall g_avs_init(SafeStruct** ppstr, char* func, char* arg, gAvsWrapperVideoFileInfo* vi, int* originalPixelType, int* originalSampleType, char* cs);
	__declspec(dllexport) int __stdcall g_avs_destroy(SafeStruct** ppstr);
	__declspec(dllexport) int __stdcall g_avs_get_last_error(SafeStruct* pstr, char* str, int len);
	__declspec(dllexport) int __stdcall g_avs_get_video_frame(SafeStruct* pstr, void* buf, int stride, int frm);
	__declspec(dllexport) int __stdcall g_avs_get_audio_frame(SafeStruct* pstr, void* buf, INT64 start, INT64 count);
	__declspec(dllexport) int __stdcall g_avs_get_int_variable(SafeStruct* pstr, const char* name, int* result);
}

/*new implementation*/

// Requires AviSynth+ >= v2.6
int __stdcall g_avs_init(SafeStruct** ppstr, char* func, char* arg, gAvsWrapperVideoFileInfo* vi, int* originalPixelType, int* originalSampleType, char* cs)
{
	SafeStruct* pstr = ((SafeStruct*)malloc(sizeof(SafeStruct)));
	*ppstr = pstr;
	memset(pstr, 0, sizeof(SafeStruct));

	pstr->dll = LoadLibrary("avisynth.dll");
	if (!pstr->dll)
	{
		strncpy_s(pstr->error_msg, ERRMSG_LEN, "Cannot load avisynth.dll!", _TRUNCATE);
		return 1;
	}

	IScriptEnvironment* (*CreateScriptEnvironment)(int version) = (IScriptEnvironment * (*)(int)) GetProcAddress(pstr->dll, "CreateScriptEnvironment");
	if (!CreateScriptEnvironment)
	{
		strncpy_s(pstr->error_msg, ERRMSG_LEN, "Cannot load CreateScriptEnvironment!", _TRUNCATE);
		return 2;
	}

	pstr->environment = CreateScriptEnvironment(AVISYNTH_INTERFACE_VERSION);
	if (pstr->environment == NULL)
	{
		strncpy_s(pstr->error_msg, ERRMSG_LEN, "Required Avisynth+!", _TRUNCATE);
		return 3;
	}

	AVS_linkage = pstr->environment->GetAVSLinkage();

	try
	{
		AVSValue arg(arg);
		AVSValue res = pstr->environment->Invoke(func, AVSValue(&arg, 1));
		if (!res.IsClip()) {
			strncpy_s(pstr->error_msg, ERRMSG_LEN, "The script's return was not a video clip!", _TRUNCATE);
			return 4;
		}
		pstr->clp = res.AsClip();
		VideoInfo inf = pstr->clp->GetVideoInfo();

		if (inf.HasVideo())
		{
			*originalPixelType = inf.pixel_type;

			if (strcmp("RGB24", cs) == 0 && (!inf.IsRGB24()))
			{
				res = pstr->environment->Invoke("ConvertToRGB24", AVSValue(&res, 1));
				pstr->clp = res.AsClip();
				//free(&inf);
				inf = pstr->clp->GetVideoInfo();

				if (!inf.IsRGB24())
				{
					//Add by Gpower2
					//Shouldn't we clear memory here?
					free(&inf);
					g_avs_destroy(ppstr);
					strncpy_s(pstr->error_msg, ERRMSG_LEN, "Cannot convert video to RGB24", _TRUNCATE);
					return	5;
				}
			}

			if (strcmp("RGB32", cs) == 0 && (!inf.IsRGB32()))
			{
				res = pstr->environment->Invoke("ConvertToRGB32", AVSValue(&res, 1));
				pstr->clp = res.AsClip();
				//free(&inf);
				inf = pstr->clp->GetVideoInfo();

				if (!inf.IsRGB32()) {
					//Add by Gpower2
					//Shouldn't we clear memory here?
					free(&inf);
					g_avs_destroy(ppstr);
					strncpy_s(pstr->error_msg, ERRMSG_LEN, "Cannot convert video to RGB32!", _TRUNCATE);
					return 5;
				}
			}

			if (strcmp("YUY2", cs) == 0 && (!inf.IsYUY2()))
			{
				res = pstr->environment->Invoke("ConvertToYUY2", AVSValue(&res, 1));
				pstr->clp = res.AsClip();
				//free(&inf);
				inf = pstr->clp->GetVideoInfo();

				if (!inf.IsYUY2())
				{
					//Add by Gpower2
					//Shouldn't we clear memory here?
					free(&inf);
					g_avs_destroy(ppstr);
					strncpy_s(pstr->error_msg, ERRMSG_LEN, "Cannot convert video to YUY2!", _TRUNCATE);
					return 5;
				}
			}

			if (strcmp("YV12", cs) == 0 && (!inf.IsYV12()))
			{
				res = pstr->environment->Invoke("ConvertToYV12", AVSValue(&res, 1));
				pstr->clp = res.AsClip();
				//free(&inf);
				inf = pstr->clp->GetVideoInfo();

				if (!inf.IsYV12())
				{
					//Add by Gpower2
					//Shouldn't we clear memory here?
					free(&inf);
					g_avs_destroy(ppstr);
					strncpy_s(pstr->error_msg, ERRMSG_LEN, "Cannot convert video to YV12!", _TRUNCATE);
					return 5;
				}
			}
		}

		if (vi != NULL)
		{
			vi->width = inf.width;
			vi->height = inf.height;
			vi->rate_numerator = inf.fps_numerator;
			vi->rate_denominator = inf.fps_denominator;
			vi->aspect_numerator = 0;
			vi->aspect_denominator = 1;
			vi->has_interlaced_frames = 0;
			vi->is_top_field_first = 0;
			vi->number_frames = inf.num_frames;
			vi->pixel_type = inf.pixel_type;

			vi->audio_samples_per_second = inf.audio_samples_per_second;
			vi->number_audio_samples = inf.num_audio_samples;
			vi->sample_type = inf.sample_type;
			vi->number_channels = inf.nchannels;
		}

		pstr->result = new AVSValue(res);

		pstr->error_msg[0] = 0;
		return 0;
	}
	catch (AvisynthError err)
	{
		strncpy_s(pstr->error_msg, ERRMSG_LEN, err.msg, _TRUNCATE);
		return 999;
	}
}

int __stdcall g_avs_get_int_variable(SafeStruct* pstr, const char* name, int* result)
{
	try
	{
		pstr->error_msg[0] = 0;
		try
		{
			AVSValue var = pstr->environment->GetVar(name);
			if (var.Defined())
			{
				if (!var.IsInt())
				{
					strncpy_s(pstr->error_msg, ERRMSG_LEN, "Variable is not Integer!", _TRUNCATE);
					return -2;
				}
				*result = var.AsInt();
				return 0;
			}
			else
			{
				return 999; // Signal "Not defined"
			}
		}
		catch (AvisynthError err)
		{
			strncpy_s(pstr->error_msg, ERRMSG_LEN, err.msg, _TRUNCATE);
			return -1;
		}
	}
	catch (IScriptEnvironment::NotFound)
	{
		return 666; // Signal "Not Found"
	}
}

int __stdcall g_avs_get_audio_frame(SafeStruct* pstr, void* buf, INT64 start, INT64 count)
{
	try
	{
		pstr->clp->GetAudio(buf, start, count, pstr->environment);
		pstr->error_msg[0] = 0;
		return 0;
	}
	catch (AvisynthError err)
	{
		strncpy_s(pstr->error_msg, ERRMSG_LEN, err.msg, _TRUNCATE);
		return -1;
	}
}

int __stdcall g_avs_get_video_frame(SafeStruct* pstr, void* buf, int stride, int frame_number)
{
	try
	{
		//Get video frame pointer
		PVideoFrame frame_pointer = pstr->clp->GetFrame(frame_number, pstr->environment);
		if (buf && stride)
		{
			pstr->environment->BitBlt((BYTE*)buf, stride, frame_pointer->GetReadPtr(), frame_pointer->GetPitch(),
				frame_pointer->GetRowSize(), frame_pointer->GetHeight());
		}
		pstr->error_msg[0] = 0;
	}
	catch (AvisynthError err)
	{
		strncpy_s(pstr->error_msg, ERRMSG_LEN, err.msg, _TRUNCATE);
		return -1;
	}
	return 0;
}

int __stdcall g_avs_get_last_error(SafeStruct* pstr, char* str, int len)
{
	strncpy_s(str, len, pstr->error_msg, len - 1);
	return (int)strlen(str);
}

int __stdcall g_avs_destroy(SafeStruct** ppstr)
{
	if (!ppstr)
	{
		return 0;
	}

	SafeStruct* pstr = *ppstr;
	if (!pstr)
	{
		return 0;
	}

	if (pstr->clp)
	{
		pstr->clp = NULL;
	}

	if (pstr->result)
	{
		delete pstr->result;
		pstr->result = NULL;
	}

	if (pstr->environment)
	{
		pstr->environment->DeleteScriptEnvironment();
		pstr->environment = NULL;
		AVS_linkage = NULL;

		/* For AviSynth v2.5
		pstr->environment->~IScriptEnvironment();
		pstr->environment = NULL;
		*/
	}

	if (pstr->dll)
	{
		FreeLibrary(pstr->dll);
	}

	free(pstr);
	*ppstr = NULL;
	return 0;
}