﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceAFIS.Matching
{
    public sealed class EdgeHash
    {
        internal Dictionary<int, object> Hash = new Dictionary<int, object>();

        public EdgeHash(FingerprintTemplate template)
        {
            for (int referenceMinutia = 0; referenceMinutia < template.Minutiae.Count; ++referenceMinutia)
                for (int neighborMinutia = 0; neighborMinutia < template.Minutiae.Count; ++neighborMinutia)
                    if (referenceMinutia != neighborMinutia)
                    {
                        var edge = new IndexedEdge()
                        {
                            Shape = EdgeConstructor.Construct(template, referenceMinutia, neighborMinutia),
                            Location = new EdgeLocation(referenceMinutia, neighborMinutia)
                        };
                        foreach (var hash in EdgeLookup.HashCoverage(edge.Shape))
                        {
                            object value;
                            List<IndexedEdge> list;
                            if (!Hash.TryGetValue(hash, out value))
                                Hash[hash] = edge;
                            else
                            {
                                list = value as List<IndexedEdge>;
                                if (list == null)
                                {
                                    Hash[hash] = list = new List<IndexedEdge>(1);
                                    list.Add((IndexedEdge)value);
                                }
                                list.Add(edge);
                            }
                        }
                    }
        }
    }
}