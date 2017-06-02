using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.GravGame
{
	public interface IBlockSequencer 
	{
		Shape.AbstractShapeDefn GetNextShapeDefn( );
		int NumShapes( );

		bool IsFinished( );
		bool Restart( );

	}

}
