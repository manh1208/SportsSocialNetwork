CREATE FULLTEXT INDEX ON [dbo].[Group]
    ([Name] LANGUAGE 1066)
    KEY INDEX [PK_Group]
    ON [fulltextsearchgroup];


GO
CREATE FULLTEXT INDEX ON [dbo].[AspNetUsers]
    ([Email] LANGUAGE 1066, [UserName] LANGUAGE 1066, [FullName] LANGUAGE 1066)
    KEY INDEX [PK_dbo.AspNetUsers]
    ON [fulltextsearchname];

