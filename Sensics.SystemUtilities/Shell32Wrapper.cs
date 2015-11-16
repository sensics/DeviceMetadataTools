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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensics.SystemUtilities
{
    public class Shell32InstanceWrapper
    {
        /// <summary>
        /// The simple way of var sh = new Shell32.Shell(); is broken in win8.
        /// see http://stackoverflow.com/a/12077409/265522
        /// </summary>
        public Shell32InstanceWrapper()
        {
            Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
            _shell = Activator.CreateInstance(shellAppType);
        }

        public Shell32.Folder NameSpace(string folder)
        {
            return _shell.NameSpace(folder);
        }

        private dynamic _shell;
    }
}
