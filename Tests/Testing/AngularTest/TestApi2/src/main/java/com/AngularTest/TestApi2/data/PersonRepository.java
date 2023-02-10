package com.AngularTest.TestApi2.data;

import org.springframework.data.jpa.repository.JpaRepository;
import com.AngularTest.TestApi2.domain.models.Person;
import com.AngularTest.TestApi2.intent.IntentMerge;
import java.util.UUID;

/**
 * Spring Data JPA repository for the Person entity.
 */
@IntentMerge
public interface PersonRepository extends JpaRepository<Person, UUID> {
}