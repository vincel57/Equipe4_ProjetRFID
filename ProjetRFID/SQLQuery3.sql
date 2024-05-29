SELECT * FROM dbo.Simulation LEFT OUTER JOIN dbo.Random_Forest 
				ON(idR=Random_Forest.id)Left OUTER JOIN dbo.Analytique ON (idA=Analytique.id)
				Left OUTER JOIN dbo.SVM ON (idS=SVM.id)
				Left OUTER JOIN dbo.KNN ON (idk=KNN.id)
				;
