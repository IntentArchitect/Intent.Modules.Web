import { Entity, Column, ObjectIdColumn, PrimaryGeneratedColumn } from 'typeorm';

@Entity('person')
export class Person {

  @Column('string')
  @ObjectIdColumn()
  @PrimaryGeneratedColumn('uuid')
  id?: string;
  @Column()
  name: string;

  @Column({ nullable: true })
  createdBy?: string;

  @Column({ nullable: true })
  createdDate?: Date;

  @Column({ nullable: true })
  lastModifiedBy?: string;

  @Column({ nullable: true })
  lastModifiedDate?: Date;
}