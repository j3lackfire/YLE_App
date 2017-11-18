using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class YLEHelper {
    private static string baseURL = "https://external.api.yle.fi/v1/programs/items.json?";
    private static string baseImageUrl = "http://images.cdn.yle.fi/image/upload/";
    private static string yleAreenaURL = "https://areena.yle.fi/";


    private static string personalKey = "app_id=6a9f2d9b&app_key=56d204570819f80ab478e90e0743bf88";
    private static string secretKey = "ce20add99825303b";

    public static string GetPersonalKey() { return personalKey; }
    public static string GetSecretKey() { return secretKey; }

    public static string GetBaseURL() { return baseURL; }

    public static string AddAuthorization(this string _baseURL)
    {
        return _baseURL + personalKey;
    }

    public static string SetSearchQuery(this string _baseURL, string _searchQuery)
    {
        return string.IsNullOrEmpty(_searchQuery) ? _baseURL : _baseURL + "q=" + _searchQuery + "&";
    }

    public static string SetSearchLimit(this string _baseURL, int _limit)
    {
        return _baseURL + "limit=" + _limit + "&";
    }

    public static string SetSearchOffset(this string _baseURL, int _offset)
    {
        return _baseURL + "offset=" + _offset + "&";
    }

    public static string GetImage(string _imageID)
    {
        return baseImageUrl + _imageID + ".png";
    }

    public static string GetImage(int _width, int _height, string _imageID)
    {
        return baseImageUrl + "w_" + _width + ",h_" + _height + ",c_fit/" + _imageID + ".png";
    }

    public static string SetTimeOrderDescending(this string _baseURL)
    {
        return _baseURL + "order=publication.starttime:desc&";
    }

    public static string GetYleUrlByID(string _contentID)
    {
        return yleAreenaURL + _contentID;
    }
}
