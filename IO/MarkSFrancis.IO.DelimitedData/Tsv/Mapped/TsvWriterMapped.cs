﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using MarkSFrancis.IO.DelimitedData.Maps;

namespace MarkSFrancis.IO.DelimitedData.Tsv.Mapped
{
    public class TsvWriterMapped<T> : TsvWriter where T : new()
    {
        private NameMap<T> Map { get; }

        public TsvWriterMapped(string fileLocation, bool autoMapProperties = true, bool autoMapFields = false)
            : base(fileLocation)
        {
            Map = AutoMap(autoMapProperties, autoMapFields);

            Map = new NameMap<T>();
            var columnHeadings = Map.GetColumnHeadings();

            SetColumnHeadings(columnHeadings.ToArray());
        }

        public TsvWriterMapped(Stream source, bool closeStreamWhenDisposed = false, bool autoMapProperties = true, bool autoMapFields = false)
            : base(source, closeStreamWhenDisposed)
        {
            Map = AutoMap(autoMapProperties, autoMapFields);

            var columnHeadings = Map.GetColumnHeadings();
            SetColumnHeadings(columnHeadings.ToArray());
        }

        public TsvWriterMapped(Stream source, NameMap<T> map, bool closeSourceWhenDisposed)
            : base(source, closeSourceWhenDisposed)
        {
            var headers = map.GetColumnHeadings();
            SetColumnHeadings(headers.ToArray());

            Map = map;
        }

        private NameMap<T> AutoMap(NameMap<T> map, bool autoMapProperties, bool autoMapFields)
        {
            if (autoMapProperties)
            {
                map.AutoMapProperties();
            }

            if (autoMapFields)
            {
                map.AutoMapFields();
            }

            return map;
        }

        private NameMap<T> AutoMap(bool autoMapProperties, bool autoMapFields) =>
            AutoMap(new NameMap<T>(), autoMapProperties, autoMapFields);

        public void WriteRecord(T data)
        {
            IList<string> values;
            lock (Map)
            {
                values = Map.MapFromObject(data);
            }

            WriteRecord(values);
        }
    }
}