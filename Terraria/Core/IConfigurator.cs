using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core
{
	public interface IConfigurator
	{
		int Music { get; set; }
		int Value { get; set; }

		bool FullScreen { get; set; }

		string NickName1 { get; set; }
		string NickName2 { get; set; }

		void LoadConfiguration();
		void SaveConfiguration();
		void ResetConfiguration();
	}
}
