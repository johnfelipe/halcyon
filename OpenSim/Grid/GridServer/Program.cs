/*
 * Copyright (c) InWorldz Halcyon Developers
 * Copyright (c) Contributors, http://opensimulator.org/
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSim Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Net;
using log4net.Config;
using log4net;
using Nini.Config;

namespace OpenSim.Grid.GridServer
{
    public static class Program
    {
		private static readonly ILog m_log = LogManager.GetLogger("OpenSim.Grid.GridServer");

        public static void Main(string[] args)
        {
            // Under any circumstance other than an explicit exit the exit code should be 1.
            Environment.ExitCode = 1;

            ServicePointManager.DefaultConnectionLimit = 12;

			m_log.Info ("starting up");

            XmlConfigurator.Configure();

			// Add the arguments supplied when running the application to the configuration
			ArgvConfigSource configSource = new ArgvConfigSource(args);

			configSource.Alias.AddAlias("On", true);
			configSource.Alias.AddAlias("Off", false);
			configSource.Alias.AddAlias("True", true);
			configSource.Alias.AddAlias("False", false);
			configSource.Alias.AddAlias("Yes", true);
			configSource.Alias.AddAlias("No", false);

			configSource.AddSwitch("Startup", "background");

			bool background = configSource.Configs["Startup"].GetBoolean("background", false);
           
			if (background) {
				m_log.Info ("[GridServer MAIN]: set to background");
				GridServerBackground app = new GridServerBackground ();
				app.Startup();
				app.Work();
			} else {
				m_log.Info ("[GridServer MAIN]: set to foreground");
				GridServerBase app = new GridServerBase();
				app.Startup();
				app.Work();
			}
        }
    }
}
