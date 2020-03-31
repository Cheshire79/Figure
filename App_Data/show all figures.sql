        select st.id as StoreId, cf.Id_Figure as FigureId, ci.Id as Id, st.Name , f.Name,'Circle' as type--, ci.Radius
		 from Store st
    join FigureStore cf on
    cf.Id_Store= st.Id
    join Figure f 
    on f.Id = cf.Id_Figure
    join Circle ci on
    f.Id_Circle= ci.Id
	union
	    select st.id as StoreId, cf.Id_Figure as FigureId, re.Id as Id, st.Name , f.Name,'Rectangle' as type--, ci.Radius
		 from Store st
    join FigureStore cf on
    cf.Id_Store= st.Id
    join Figure f 
    on f.Id = cf.Id_Figure
    join Rectangle re on
    f.Id_Rectangle = re.Id
	union
	  select st.id as StoreId, cf.Id_Figure as FigureId, sq.Id as Id, st.Name , f.Name,'Square' as type--, ci.Radius
		 from Store st
    join FigureStore cf on
    cf.Id_Store= st.Id
    join Figure f 
    on f.Id = cf.Id_Figure
    join Square sq on
    f.Id_Square = sq.Id


	  