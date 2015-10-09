using System;
using UIKit;

namespace Solvberget.iOS
{
	public interface ISimpleCell{
		void Bind(string title, string subtitle);
		void SetImage(UIImage image);
		string ImageUrl{get;set;}

		nfloat TableWidth { get; set;}
	}
	
}
