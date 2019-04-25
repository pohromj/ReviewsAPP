CREATE TABLE Header_row_data (
    id int NOT NULL identity(1,1),
    value varchar(250),
	Review_id int not null,
	Header_row_id int not null
    CONSTRAINT PK_Header_row_data PRIMARY KEY (ID)
);

alter table Header_row_data
add constraint Review_FK
Foreign key (Review_id) references Review(id)

alter table Header_row_data
add constraint Header_row_FK
Foreign key (Header_row_id) references Header_row(id)

select * from Header_row_data