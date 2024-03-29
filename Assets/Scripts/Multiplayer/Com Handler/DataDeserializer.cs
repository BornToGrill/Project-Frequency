﻿using System;

internal static class DataDeserializer {

    private const char DataSeparator = ':';

    internal static SplitData GetFirst(this string data) {
        int index = data.IndexOf(DataSeparator);
        if (index < 0)
            return new SplitData() { CommandType = data, Values = "" };
        return new SplitData() { CommandType = data.Substring(0, index), Values = data.Substring(index + 1) };
    }
}

internal class SplitData {
    internal string CommandType { get; set; }
    internal string Values { get; set; }
}
