package com.AngularTest.TestApi2.domain.models;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import com.AngularTest.TestApi2.intent.IntentManageClass;
import com.AngularTest.TestApi2.intent.Mode;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Table;

@Entity
@Table(name = "people")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class Person extends AbstractEntity {
    private static final long serialVersionUID = 1L;

    @Column(name = "name", nullable = false)
    private String name;
}
