CREATE TABLE [dbo].[Food] ( 
	[id] INT IDENTITY (1, 1) NOT NULL, 
	[description] NVARCHAR (200) NULL, 
	[measures] INT NOT NULL,
	CONSTRAINT [PK_dbo.CaloriesDiary] PRIMARY KEY CLUSTERED ([id] ASC)
); 
 
CREATE TABLE [dbo].[Measure] ( 
	[id] INT IDENTITY (1, 1) NOT NULL, 
	[description] NVARCHAR (200) NULL, 
	[calories] int NULL, 
	[fat] INT NOT NULL, 
	[foodid] INT NOT NULL,
	CONSTRAINT [PK_dbo.Measure] PRIMARY KEY CLUSTERED ([id] ASC), 
   CONSTRAINT [FK_dbo.Food_dbo.Measure] FOREIGN KEY ([foodid]) REFERENCES [dbo].[Food] ([id]) ON DELETE CASCADE  
);

CREATE TABLE [dbo].[Diary] ( 
	[id] INT IDENTITY (1, 1) NOT NULL, 
	[date] NVARCHAR (200) NULL, 
	[username] NVARCHAR (200) NULL,
	CONSTRAINT [PK_dbo.Diary] PRIMARY KEY CLUSTERED ([id] ASC)
	
); 
 
create TABLE [dbo].[DiaryEntry] ( 
	[id] INT IDENTITY (1, 1) NOT NULL, 
	[quantity] INT NULL,
	[foodid] INT NOT NULL,
	[diaryid] INT NOT NULL,
	[measureid] INT NOT NULL,
	CONSTRAINT [PK_dbo.DiaryEntry] PRIMARY KEY CLUSTERED ([id] ASC),
	CONSTRAINT [FK_dbo.Food_dbo.DiaryEntry_id] FOREIGN KEY ([foodid]) REFERENCES [dbo].[Food] ([id]) ON DELETE CASCADE,
	CONSTRAINT [FK_dbo.Diary_dbo.DiaryEntry] FOREIGN KEY ([diaryid]) REFERENCES [dbo].[Diary] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_dbo.Measure_dbo.DiaryEntry] FOREIGN KEY ([measureid]) REFERENCES [dbo].[Measure] ([id]) ON DELETE CASCADE    
	
); 

--ALTER TABLE DiaryEntry
--add CONSTRAINT Food_DiaryEntry_FK FOREIGN KEY(foodid) REFERENCES Food(id)
--    ON DELETE NO ACTION; 
