< div >
                < h2 > Medewerker Toevoegen </ h2 >
                < input
                    type = "text"
                    placeholder = "Name"
                    value ={ name}
onChange ={ (e) => setName(e.target.value)}
                />
                < input
                    type = "email"
                    placeholder = "Email"
                    value ={ email}
onChange ={ (e) => setEmail(e.target.value)}
                />
                < input
                    type = "text"
                    placeholder = "Company ID"
                    value ={ companyId}
onChange ={ (e) => setCompanyId(e.target.value)}
                />
                < button onClick ={ handleAddEmployee}> Add Employee </ button >
            </ div >