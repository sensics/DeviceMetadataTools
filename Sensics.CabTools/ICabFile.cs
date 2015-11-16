#region copyright
// Copyright 2015 Sensics, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
using System.IO;

namespace Sensics.CabTools
{
    /// <summary>
    /// Interface allowing you to read the context of a text file contained in a cab.
    /// Implementations may open the cab but may not hold it open or hold it exclusively.
    /// </summary>
    public interface ICabFile
    {
        TextReader OpenTextFile(string contained);
    }
}
