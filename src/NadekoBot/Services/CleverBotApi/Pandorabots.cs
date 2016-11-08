using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/*
    ChatterBotAPI
    Copyright (C) 2011 pierredavidbelanger@gmail.com
 
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace Services.CleverBotApi
{
    internal class Pandorabots : ChatterBot
    {
        private readonly string botid;

        public Pandorabots(string botid)
        {
            this.botid = botid;
        }

        public ChatterBotSession CreateSession()
        {
            return new PandorabotsSession(botid);
        }
    }

    internal class PandorabotsSession : ChatterBotSession
    {
        private readonly IDictionary<string, string> vars;

        public PandorabotsSession(string botid)
        {
            vars = new Dictionary<string, string>();
            vars["botid"] = botid;
            vars["custid"] = Guid.NewGuid().ToString();
        }

        public async Task<ChatterBotThought> Think(ChatterBotThought thought)
        {
            vars["input"] = thought.Text;

            var response = await Utils.Post("http://www.pandorabots.com/pandora/talk-xml", vars, null).ConfigureAwait(false);

            var responseThought = new ChatterBotThought();
            responseThought.Text = Utils.XPathSearch(response, "//result/that/text()");

            return responseThought;
        }

        public async Task<string> Think(string text)
        {
            return (await Think(new ChatterBotThought {Text = text}).ConfigureAwait(false)).Text;
        }
    }
}